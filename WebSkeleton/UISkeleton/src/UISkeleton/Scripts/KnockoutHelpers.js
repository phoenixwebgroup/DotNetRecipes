$.knockoutHelpers = {}
$.knockoutHelpers.ValueUpdate = {
    AfterKeyDown: 'afterkeydown',
    Change: 'change',
    KeyUp: 'keyup',
    KeyDown: 'keydown',
}


/** Binding to make content appear with 'fade' effect */
ko.bindingHandlers.fadeIn = {
	update: function (element, valueAccessor, allBindingsAccessor) {
		var value = valueAccessor(), allBindings = allBindingsAccessor();
		var valueUnwrapped = ko.utils.unwrapObservable(value);
		if (valueUnwrapped === true) {
			var speed = allBindings.speed || 'fast';
			$(element).fadeIn();
		}
	}
};
/** Binding to make content disappear with 'fade' effect */
ko.bindingHandlers.fadeOut = {
	update: function (element, valueAccessor, allBindingsAccessor) {
		var value = valueAccessor(), allBindings = allBindingsAccessor(); ;
		var valueUnwrapped = ko.utils.unwrapObservable(value);
		if (valueUnwrapped === true) {
			var speed = allBindings.speed || 'fast';
			$(element).fadeOut();
		}
	}
};

// jqModal dialog
ko.bindingHandlers.modal = {
	init: function(element, valueAccessor, allBindingsAccessor, context) {
		var options = valueAccessor();
		$(element).addClass("jqmWindow");		// why isn't this automatic in jqm?
		$(element).jqm({
			onHide: function(hash) { 
				// hide the dialog; jqm doesn't invoke it's own onHide method since this is set.
				// remove children elements; otherwise they show up next time window opens and before it's rebound.
				hash.w.fadeOut(100,function(){ hash.o.remove(); $(element).empty(); });
				// mark window as hidden
				options.visible(false);
			}
		});
	},
	update: function(element, valueAccessor) {
		var options = valueAccessor();
		var show = ko.utils.unwrapObservable(options.visible);
		if(show) {
			$(element).jqmShow();
			var model = options.getData();
			ko.renderTemplate(options.templateName, model, {}, element, "replaceChildren"); 
		}
		// if we haven't hidden the popup, hide it
		else if($(element).is(":visible")) $(element).jqmHide();
	}
};
	
// sets observable to true when element is clicked
ko.bindingHandlers.trueOnClick = {
	init: function(element, valueAccessor, allBindingsAccessor, context) {
		var newValueAccessor = function() {
			return function() {
				valueAccessor()(true);
			};
    };
		ko.bindingHandlers.click.init(element, newValueAccessor, allBindingsAccessor, context);
	}
};

/** Use existing text */
ko.bindingHandlers.keepValue = {
	init: function (element, valueAccessor, allBindingsAccessor) {
		var value = $(element).val();
		if (value != "" && value != undefined) {
			valueAccessor()(value);
		}
		ko.bindingHandlers.value.init(element, valueAccessor, allBindingsAccessor);
	}
};

// jquery autocomplete
ko.bindingHandlers.autocomplete = {
	init: function(element, valueAccessor, allBindingsAccessor, context) {
		var options = valueAccessor();
		$(element).autocomplete(options.url);
	    $(element).result(function(event, data, formatted) {
	        if (data != undefined)
	        {
	            options.text(data[0]);
	            options.value(data[1]);
	        }
	        else {
	            alert("undef")
	        }
	    });
	},
    // todo change this
	update: function(element, valueAccessor) {
	   // $(element).text(valueAccessor().text());
//		var options = valueAccessor();
//		var show = ko.utils.unwrapObservable(options.visible);
//		if(show) {
//			$(element).jqmShow();
//			var model = options.getData();
//			ko.renderTemplate(options.templateName, model, {}, element, "replaceChildren"); 
//		}
	}
};