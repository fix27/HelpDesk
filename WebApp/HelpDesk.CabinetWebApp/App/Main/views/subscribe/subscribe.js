(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.subscribe.subscribe';
    app.controller(controllerId, [
        '$scope', '$rootScope', 'userService', 
        function ($scope, $rootScope, userService) {

            var vm = this;
                        
            vm.changeSubscribeRequestState = function (requestStateId) {
                vm.loadingFlag = true;
                userService.changeSubscribeRequestState(requestStateId).then(function (results) {
                    vm.errors = {};
                    vm.showAlert = true;
                    if (results.data.success === false) {
                        vm.errors = results.data.errors;
                    }
                    else {
                        $scope.$broadcast("editModeEvent", { editMode: false });
                    }
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });
            };

            vm.changeSubscribe = function () {
                vm.loadingFlag = true;
                userService.changeSubscribe().then(function (results) {
                    vm.errors = {};
                    vm.showAlert = true;
                    if (results.data.success === false) {
                        vm.errors = results.data.errors;
                    }
                    else {
                        $scope.$broadcast("editModeEvent", { editMode: false });
                    }
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });
            };
                        
            var init = function () {
                
                userService.getListSubscribeStatus().then(function (results) {
                    vm.listSubscribe = results.data.data;
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

                userService.get().then(function (results) {
                    vm.user = results.data.data;
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });
                                
                vm.errors = {};
            }
            
            init();
        }
    ]);
})();