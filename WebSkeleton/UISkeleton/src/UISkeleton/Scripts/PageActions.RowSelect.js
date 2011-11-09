$.pageActions.RowSelect =
	{
		For: function (tableSelector, options) {
			$.pageActions.GetSelectedRecordData = $.pageActions.RowSelect.GetSelectedRecordData;
			$.pageActions.GetDisabledActions = $.pageActions.RowSelect.GetDisabledActions;
			$.pageActions.RowSelect.TableSelector = tableSelector;
			$.pageActions.RowSelect.AttributeName = options.attrName;

			$(function () {
				var settings = {
					allowMulti: true,
					selectionChanged: function (values) {
						$.pageActions.SelectionChanged();
					}
				};
				$.extend(settings, options);
				$(tableSelector).rowSelect(settings);
				$.pageActions.SelectionChanged();
			});
		},
		GetSelectedRecordDataFor: function (selector) {
			var selectedValues = $.rowSelect.api.getSelectedValues(selector);
			if (selectedValues == undefined) {
				return null;
			}

			var values = JSLINQ(selectedValues);

			if (values.Count() == 0) {
				return null;
			}
			var first = $.pageActions.RowSelect.RowKeySelector(values.First());
			if ($.rowSelect.api.getSettings(selector).allowMulti == true) {
				return $.extend(first, { ids: values.Select(RowKeySelector).ToArray() });
			}
			return first;
		},
		GetSelectedRecordData: function () {
			var selector = $.pageActions.RowSelect.TableSelector;
			return $.pageActions.RowSelect.GetSelectedRecordDataFor(selector);
		},
		RowKeySelector: function (key) {
			return $.evalJSON(key);
		},
		GetDisabledActions: function () {
			return JSLINQ(jQuery.rowSelect.api.getSelected($.pageActions.RowSelect.TableSelector))
				.Where(function (r) { return $(r).attr('disablePageActionsList') != undefined; })
				.SelectMany(function (r) { return $(r).attr('disablePageActionsList').split(' '); })
				.Distinct()
				.ToArray();
		}
	};