(function () {
	function getColumnsForScaffolding(data) {
		if ((typeof data.length != 'number') || data.length == 0)
			return [];
		var columns = [];
		for (var propertyName in data[0])
			columns.push({ headerText: propertyName, rowText: propertyName, sortable: false }); // todo scaffold.sortable?, index conventions
		return columns;
	}

	var resultMapping = {
		'rows': {
			key: function (data) {
				return ko.utils.unwrapObservable(data.Key);
			}
		}
	};

	var defaultRowModifiers = {
		'class': function (row) { return ""; } // must use quotes for IE compatability
	};

	ko.dynamicGrid = {
		viewModel: function (configuration) {
			this.currentPageIndex = ko.observable(0);
			this.pageSize = configuration.pageSize || 20;
			this.tableId = configuration.tableId || "results";

			this.sidx = configuration.sidx || "Id";
			this.sord = configuration.sord || "asc";

			if (configuration.query || configuration.url != undefined) {
				var mapping = configuration.resultMapping || resultMapping;
				this.data = ko.mapping.fromJS({ rows: [] }, mapping);
				this.query = configuration.query || null;
				this.rowModifiers = $.extend(defaultRowModifiers, configuration.rowModifiers || {});
				var _runningSearch = null;
				var executeSearch = function (query, csv) {
					if (_runningSearch != null) {
						_runningSearch.abort();
					}
					if (query == "") {
						this.data.rows.removeAll();
						this.totalItems(0);
						return;
					}
					that = this;
					var query = ko.utils.unwrapObservable(query) || "";
					var searchData = { query: query, rows: this.pageSize, page: this.currentPageIndex() + 1, t: new Date().getTime(), sidx: this.sidx, sord: this.sord };
					if (configuration.queryParams) {
						searchData = $.extend(searchData, ko.utils.unwrapObservable(configuration.queryParams));
					}
					if (csv) {
						searchData = $.extend(searchData, { outputType: "Csv" });
						$.pageActions.OpenNewWindowFromBase(configuration.url + "?" + $.param(searchData));
					}
					else {
						_runningSearch = $.ajax({
							url: configuration.url,
							type: "GET",
							data: searchData,
							success: function (data) {
								ko.mapping.updateFromJS(that.data, data);
								that.totalItems(data.records);
								if (configuration.query) $("table#" + that.tableId + " tbody").removeHighlight().highlightAll(query);
							},
							dataType: "json",
							error: $.ajaxFormsExtensions.AjaxForm.Error,
							complete: function () {
								that._runningSearch = null;
							}
						});
					}
				} .bind(this);

				this.itemsOnCurrentPage = ko.dependentObservable(function () {
					return ko.utils.unwrapObservable(this.data).rows();
				}, this);

				this.totalItems = ko.observable(0);

				this.GoToPage = function (i) {
					this.currentPageIndex(i);
					executeSearch(ko.utils.unwrapObservable(this.query));
				} .bind(this);

				this.SortBy = function (e) {
					e.preventDefault();
					var sortBy = $(e.target).attr("data-sidx");
					if (this.sidx == sortBy) {
						if (this.sord == "asc") {
							this.sord = "desc";
						} else {
							this.sord = "asc";
						}
					}
					else {
						this.sidx = sortBy;
					}
					this.GoToPage(0);
				} .bind(this);

				// rerun search when any observable queryParams' member changes
				if (configuration.queryParams) {
					this._filterUpdate = ko.dependentObservable(function () {
						ko.toJS(configuration.queryParams);
					}, this).subscribe(function () {
						this.GoToPage(0);
					} .bind(this));
					//for (var propertyName in configuration.queryParams) {
					//	if (typeof configuration.queryParams[propertyName].subscribe == 'function') {
					//		configuration.queryParams[propertyName].subscribe(function () {
					//			this.GoToPage(0);
					//		}, this);
					//	}
					//}
				}

				if (this.query && this.query.subscribe) {
					this.query.subscribe(function (q) {
						this.currentPageIndex(0);
						executeSearch(q);
					}, this);
				}
			}
			else {
				this.data = configuration.data;
				this.itemsOnCurrentPage = ko.dependentObservable(function () {
					var startIndex = this.pageSize * this.currentPageIndex();
					return this.data.slice(startIndex, startIndex + this.pageSize);
				}, this);
				this.totalItems = ko.observable(ko.utils.unwrapObservable(this.data).length);
				this.GoToPage = function (i) {
					this.currentPageIndex(i);
				} .bind(this);
			}

			// If you don't specify columns configuration, we'll use scaffolding
			this.columns = configuration.columns || getColumnsForScaffolding(ko.utils.unwrapObservable(this.data));
			this.header = configuration.header || [];

			this.maxPageIndex = ko.dependentObservable(function () {
				return Math.ceil(ko.utils.unwrapObservable(this.totalItems) / this.pageSize);
			}, this);

			if (configuration.query == undefined && configuration.url != undefined) {
				executeSearch(null);
			}

			var rebuildPager = function () {
				$("#" + this.tableId + "_pager").pagination(this.totalItems(), {
					num_edge_entries: 2,
					num_display_entries: 8,
					callback: function (page) { this.GoToPage(page); return false; } .bind(this),
					items_per_page: this.pageSize
				});
			} .bind(this);

			this.currentPageIndex.subscribe(function (page) {
				if (page == 0) rebuildPager();
			}, this);

			this.totalItems.subscribe(function () {
				rebuildPager();
			}, this);

			this.getCsv = function (e) {
				e.preventDefault();
				if (configuration.data) {
					alert("Sorry, CSV is not yet supported on this table.")
				}
				executeSearch(ko.utils.unwrapObservable(this.query), true);
			};

			this.showOptions = function (e) {
				e.preventDefault();
				alert("Coming soon!");
			};
		}
	};

	// Templates used to render the grid
	var templateEngine = new ko.jqueryTmplTemplateEngine();
	templateEngine.addTemplate("ko_dynamicGrid_grid", "\
                    <table id=\"{{= tableId }}\" class=\"report\" cellspacing=\"0\">\
                        <thead>\
							{{if header.length > 0}}\
							<tr>\
                                {{each(i, columnDefinition) header}}\
                                    <th colspan=\"${ columnDefinition.colspan }\">${ columnDefinition.headerText }</th>\
                                {{/each}}\
                            </tr>\
							{{/if}}\
                            <tr>\
                                {{each(i, columnDefinition) columns}}\
									{{if columnDefinition.sortable }}\
										<th><a href=\"#\" data-sidx=\"{{= index }}\" data-bind=\"click: SortBy\">${ columnDefinition.headerText }</a></th>\
									{{else}}\
										<th>${ columnDefinition.headerText }</th>\
									{{/if}}\
                                {{/each}}\
                            </tr>\
                        </thead>\
                        <tbody data-bind=\"template: { name: 'ko_dynamicGrid_row', foreach: itemsOnCurrentPage, templateOptions: { columns: columns, rowModifiers: rowModifiers } }\">\
                        </tbody>\
                    </table>\
					<div class=\"pager_container\" id=\"{{= tableId}}_pager\"></div>\
					<div class=\"gridoptions\">\
						<a href=\"#\" data-bind=\"click: getCsv\">Download CSV</a>\
						<a href=\"#\" data-bind=\"click: showOptions\">Options</a>\
					</div>\
					<div style=\"reset: both\"></div>");
	templateEngine.addTemplate("ko_dynamicGrid_row", "\
                    <tr class=\"${ $item.rowModifiers['class']($data) }\">\
                        {{each(j, columnDefinition) $item.columns}}\
							{{if columnDefinition.rowHtml != undefined }}\
								<td>{{html columnDefinition.rowHtml($data) }}</td>\
                            {{else}}\
								<td>${ typeof columnDefinition.rowText == 'function' ? columnDefinition.rowText($data) : $data[columnDefinition.rowText] }</td>\
							{{/if}}\
                        {{/each}}\
                    </tr>");

	// The "dynamicGrid" binding
	ko.bindingHandlers.dynamicGrid = {
		// This method is called to initialize the node, and will also be called again if you change what the grid is bound to
		update: function (element, viewModelAccessor, allBindingsAccessor) {
			var viewModel = viewModelAccessor(), allBindings = allBindingsAccessor();

			// Empty the element
			while (element.firstChild)
				ko.removeNode(element.firstChild);

			// Allow the default templates to be overridden
			var gridTemplateName = allBindings.dynamicGridTemplate || "ko_dynamicGrid_grid",
                pageLinksTemplateName = allBindings.dynamicGridPagerTemplate || "ko_dynamicGrid_pageLinks";

			// Render the main grid
			var gridContainer = element.appendChild(document.createElement("DIV"));
			ko.renderTemplate(gridTemplateName, viewModel, { templateEngine: templateEngine }, gridContainer, "replaceNode");
		}
	};
})();