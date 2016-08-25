var SearchResultApp = (function(H, U, W) {
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
        return U.updateQueryStringParameter(url, encodeURIComponent(element.name), encodeURIComponent(element.value));
    }

    function _getRequestUrl() {
        var url = form.action;

        url = _appendElementAsQueryString(url, addressLineElement);
        url = _appendElementAsQueryString(url, cityElement);
        url = _appendElementAsQueryString(url, stateElement);
        url = _appendElementAsQueryString(url, zipCodeElement);

        if (estimateRentElement.checked) {
            url = U.updateQueryStringParameter(url, encodeURIComponent(estimateRentElement.name), "true");
        }

        return url;
    }

    function _addErrorMessage(errorMessage) {
        if (U.isNullOrWhitespace(errorMessage)) return;
        errorMessagesUl.insertAdjacentHTML("beforeend", "<li>" + errorMessage + "</li>");
    }

    function _validateForm() {
        var valid = true, errorMessages = [];

        U.removeAllChildElements(errorMessagesUl);

        if (U.isNullOrWhitespace(addressLineElement.value)) {
            valid = false;
            errorMessages.push("Address line required");
        }

        //Poor mans validation for now. 
        if (U.isNullOrWhitespace(cityElement.value + stateElement.value + zipCodeElement.value)) {
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
            //Cache template
            template = H.compile(document.getElementById("searchResultTemplate").innerHTML);
        }

        searchResultContainer.insertAdjacentHTML("beforeend", template(results));
    }

    function _handleSearchResponse() {
        var xhr = this;
        spinner.className += "hidden";
        U.removeAllChildElements(searchResultContainer);

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
        //Since there are only a handfull of elements I am caching them all upfront during initialize.
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
})(Handlebars, Utilities, window);