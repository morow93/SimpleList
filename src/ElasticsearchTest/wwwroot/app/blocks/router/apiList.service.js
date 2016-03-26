(function () {
    angular.module("blocks.router")
        .factory("apiList", apiList);

    apiList.$inject = ["MAIN_CONSTANTS"];

    function apiList(MAIN_CONSTANTS) {
        
        var prefix = MAIN_CONSTANTS.SERVER + "/api";

        var cards = prefix + "/values";
        var update = cards + "/update";
        var add = cards + "/add";
        var search = cards + "/search";

        return {
            cards: cards,
            update: update,
            add: add,
            search: search
        };
    };
})();
