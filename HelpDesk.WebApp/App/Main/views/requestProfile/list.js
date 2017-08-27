(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.requestProfile.list';
    app.controller(controllerId, [
                 '$rootScope', '$state', 'requestProfileService', 'modalService',
    function ($rootScope, $state, requestProfileService, modalService) {

        var vm = this;

        vm.orderInfo = { propertyName: "Wares", asc: false };

        vm.createRequest = function (id) {
            $state.go("request", { objectId: id });
        }

        

        vm.filter = {
            ObjectName: null,
            Wares: []
        };

        extendColumnHeader(vm);

        
        vm.loaded = false;
        vm.refresh = function () {
            vm.loaded = false;
            requestProfileService.getListPersonalObject(vm.filter, vm.orderInfo, { currentPage: vm.currentPage - 1, pageSize: vm.pageSize }).then(function (results) {
                vm.recs = results.data.data;
                vm.totalCount = results.data.totalCount;
                vm.count = results.data.count;
                vm.loaded = true;
            }, function (error) {
                $rootScope.$broadcast("error", { errorMsg: error.data.Message });
            });

        };
                
        requestProfileService.getListWare().then(function (results) {
            vm.filterWares = results.data.data;
        }, function (error) {
            $rootScope.$broadcast("error", { errorMsg: error.data.Message });
        });


        vm.addIS = function () {
            modalService.showModal({
                templateUrl: "/AngularTemplate/RequestProfileAddIS",
                controller: "app.views.requestProfile.addIS as vm",
                inputs: {
                    params: {}
                }
            }).then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {

                    if (result.cancel)
                        return;
                    vm.refresh();

                });
            });
        }

        vm.addTO = function () {
            modalService.showModal({
                templateUrl: "/AngularTemplate/RequestProfileAddTO",
                controller: "app.views.requestProfile.addTO as vm",
                inputs: {
                    params: {}
                }
            }).then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {

                    if (result.cancel)
                        return;
                    vm.refresh();

                });
            });
        }

        vm.delete = function (id)
        {
            requestProfileService.delete(id).then(function (results) {
                vm.errors = {};

                if (results.data.success === false) {
                    vm.errors = results.data.errors;
                }
                else
                {
                    vm.refresh();
                }
                
            }, function (error) {
                $rootScope.$broadcast("error", { errorMsg: error.data.Message });
            });
        }
        vm.refresh();

    }

    ]);
})();