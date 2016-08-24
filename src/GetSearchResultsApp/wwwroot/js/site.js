var SearchResultApp = (function(H, W) {
    "use strict";

    var form,
        searchButton,
        addressLineElement,
        cityElement,
        stateElement,
        zipCodeElement,
        estimateRentElement,
        errorMessagesUl,
        spinner,
        searchResultContainer,
        template,
        generalErrorMessage = "Error occured. Please try again later",
        self = { initialize: function() { return _initialize() } };


    function _appendElementAsQueryString(url, element) {
        if (!element || !url) return url;
        return Utilities.updateQueryStringParameter(url, encodeURIComponent(element.name), encodeURIComponent(element.value));
    }

    function _getRequestUrl() {
        var url = form.action;

        url = _appendElementAsQueryString(url, addressLineElement);
        url = _appendElementAsQueryString(url, cityElement);
        url = _appendElementAsQueryString(url, stateElement);
        url = _appendElementAsQueryString(url, zipCodeElement);

        if (estimateRentElement.checked) {
            url = Utilities.updateQueryStringParameter(url, encodeURIComponent(estimateRentElement.name), "true");
        }

        return url;
    }

    function _addErrorMessage(errorMessage) {
        if (Utilities.isNullOrWhitespace(errorMessage)) return;
        errorMessagesUl.insertAdjacentHTML("beforeend", "<li>" + errorMessage + "</li>");
    }

    function _validateForm() {
        var valid = true, errorMessages = [];

        Utilities.removeAllChildElements(errorMessagesUl);

        if (Utilities.isNullOrWhitespace(addressLineElement.value)) {
            valid = false;
            errorMessages.push("Address line required");
        }

        //Poor mans validation for now. 
        if (Utilities.isNullOrWhitespace(cityElement.value + stateElement.value + zipCodeElement.value)) {
            valid = false;
            errorMessages.push("More information required");
        }
        if (!valid) {
            for (var i in errorMessages) {
                if (errorMessages.hasOwnProperty(i)) {
                    _addErrorMessage(errorMessages[i]);
                }
            }
        }

        return valid;
    }

    function _handleFailureResponse(response) {
        var messages = response && response.errorMessages;
        if (messages && messages.length) {
            for (var i in messages) {
                if (messages.hasOwnProperty(i)) {
                    _addErrorMessage(messages[i]);
                }
            }
        } else {
            _addErrorMessage(generalErrorMessage);
        }
    }

    function _handleSuccessResponse(response) {
        var results = response && response.searchResults;

        if (!results || !results.length) {
            _handleFailureResponse(response);
            return;
        }

        if (!template) {
            template = H.compile(document.getElementById("searchResultTemplate").innerHTML);
        }

        searchResultContainer.insertAdjacentHTML("beforeend", template(results));
    }

    function _handleSearchResponse() {
        var xhr = this;
        spinner.className += "hidden";
        Utilities.removeAllChildElements(searchResultContainer);

        if (xhr.status !== 200) {
            _addErrorMessage(generalErrorMessage);
            return;
        }

        var response = JSON.parse(xhr.responseText);

        if (!response) {
            _addErrorMessage(generalErrorMessage);
            return;
        }

        if ((/^true$/i.test(response.limitWarning))) {
            //Not sure what else to do.
            alert("Approaching limit, slow down");
        }

        if (!(/^true$/i.test(response.success))) {
            _handleFailureResponse(response);
            return;
        }

        _handleSuccessResponse(response);
    }

    function _search(event) {

        if (!_validateForm()) {
            return;
        }
        spinner.className = spinner.className.replace(/^hidden$/, "");
        var xhr = new XMLHttpRequest();
        xhr.open("GET", _getRequestUrl());
        xhr.onload = _handleSearchResponse.bind(xhr);
        xhr.send();
    }

    function _cacheElementReferences() {
        form = document.getElementById("searchForm");
        searchButton = document.getElementById("searchButton");
        addressLineElement = document.getElementById("addressLineText");
        cityElement = document.getElementById("cityText");
        stateElement = document.getElementById("stateText");
        zipCodeElement = document.getElementById("zipCodeText");
        estimateRentElement = document.getElementById("estimateRentCheckbox");
        spinner = document.getElementById("spinner");
        searchResultContainer = document.getElementById("searchResultContainer");
        errorMessagesUl = document.querySelector("#validationMessages ul");
    }

    function _initialize() {
        _cacheElementReferences();
        form.addEventListener("submit", function(e) { e.preventDefault(); });
        searchButton.addEventListener("click", _search);
    }

    return self;
})(Handlebars, window);


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