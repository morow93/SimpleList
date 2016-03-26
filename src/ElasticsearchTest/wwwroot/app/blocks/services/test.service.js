(function () {
    angular.module("blocks.services")
        .factory("TestService", testService);

    testService.$inject = ["$q", "$http", "apiList"];

    function testService($q, $http, apiList) {

        var service = {
            getCards: getCards,
            update: update,
            add: add,
            remove: remove,
            search: search
        };

        return service;

        function getCards() {
            return $http.get(apiList.cards);
        }

        function update(params) {
            return $http.post(apiList.update, params);
        }

        function add(params) {
            return $http.post(apiList.add, params);
        }

        function remove(params) {
            return $http.delete(apiList.cards + "/" + params);
        }

        function search(params) {
            return $http.get(apiList.search + "?query=" + encodeURIComponent(params));
        }

    };
})();
