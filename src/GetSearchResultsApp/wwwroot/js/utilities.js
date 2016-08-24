var Utilities = (function(W) {
    "use strict";
    var self = {
        updateQueryStringParameter: function(uri, key, value) { return _updateQueryStringParameter(uri, key, value); },
        isNullOrWhitespace: function(input) { return _isNullOrWhitespace(input); },
        removeAllChildElements: function(element) { return _removeAllChildElements(element); }
    }

    function _isNullOrWhitespace(input) {
        return (!input) || (input.replace(/\s/g, "").length < 1);
    }

    function _updateQueryStringParameter(uri, key, value) {
        if (!uri || !key) return uri;

        //http://stackoverflow.com/a/6021027/321035
        var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
        var separator = uri.indexOf("?") !== -1 ? "&" : "?";
        if (uri.match(re)) {
            return uri.replace(re, "$1" + key + "=" + value + "$2");
        } else {
            return uri + separator + key + "=" + value;
        }
    }

    function _removeAllChildElements(element) {
        if (!element) return;
        while (element.firstChild) {
            element.removeChild(element.firstChild);
        }
    }

    return self;
})(window);