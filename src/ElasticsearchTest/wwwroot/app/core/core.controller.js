(function () {
    angular.module("TestApp.core")
        .controller("CoreController", coreController);

    coreController.$inject = ["$scope", "TestService"];

    function coreController($scope, testService) {

        var vm = this;

        vm.card = {};
        vm.cards = [];
        vm.rememberCards = [];
        vm.query = "";
        $scope.forms = {};

        vm.search = search;

        vm.startEdit = startEdit;
        vm.rejectEdit = rejectEdit;
        vm.applyEdit = applyEdit;

        vm.startAdd = startAdd;
        vm.rejectAdd = rejectAdd;
        vm.applyAdd = applyAdd;

        vm.remove = remove;

        activate();

        function activate() {
            testService.getCards().then(function (response) {
                vm.cards = _.each(response.data, function (card) { card.saved = true; });
            });
        }

        function search() {
            testService.search(vm.query).then(function (response) {
                vm.cards = response.data;
            });
        }

        function startEdit(card) {
            var rememberCard = _.find(vm.rememberCards, { id: card.id });
            if (rememberCard) {
                rememberCard = _.omit(card, ['editable']);
            }else{
                vm.rememberCards.push(_.omit(card, ['editable']))
            }
            card.editable = true;
        }

        function rejectEdit(card) {
            var rememberCard = _.find(vm.rememberCards, { id: card.id });
            if (rememberCard) {
                card.title = rememberCard.title;
                card.text = rememberCard.text;
            }
            card.editable = false;
        }

        function applyEdit(card) {
            var form = $scope.forms["card" + card.id];
            if (form.$valid) {
                testService.update(card).then(function (response) {
                    card.editable = false;
                });
            }
        }

        function startAdd() {
            vm.showForm = true;
        }

        function rejectAdd() {
            vm.showForm = false;
            vm.card = {};
        }

        function applyAdd(form) {
            if (form.$valid) {
                testService.add(vm.card).then(function (response) {
                    vm.showForm = false;
                    vm.card = {};
                    vm.cards.unshift(response.data);
                });
            } else {
                _.each(form.$error, function (field) {
                    _.each(field, function (errorField) {
                        errorField.$setTouched();
                    });
                });
            }
        }

        function remove(card) {
            testService.remove(card.id).then(function (response) {
                _.remove(vm.cards, function (c) {
                    return c.id == card.id;
                });
            });
        }
    };
})();
