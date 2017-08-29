(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.employeeObject.list';
    app.controller(controllerId, [
                 '$rootScope', '$state', 'employeeObjectService', 'modalService',
    function ($rootScope, $state, employeeObjectService, modalService) {

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
            employeeObjectService.getListEmployeeObject(vm.filter, vm.orderInfo, { currentPage: vm.currentPage - 1, pageSize: vm.pageSize }).then(function (results) {
                vm.recs = results.data.data;
                vm.totalCount = results.data.totalCount;
                vm.count = results.data.count;
                vm.loaded = true;
            }, function (error) {
                $rootScope.$broadcast("error", { errorMsg: error.data.Message });
            });

        };
                
        employeeObjectService.getListWare().then(function (results) {
            vm.filterWares = results.data.data;
        }, function (error) {
            $rootScope.$broadcast("error", { errorMsg: error.data.Message });
        });


        vm.addIS = function () {
            modalService.showModal({
                templateUrl: "/AngularTemplate/EmployeeObjectAddIS",
                controller: "app.views.employeeObject.addIS as vm",
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
                templateUrl: "/AngularTemplate/EmployeeObjectAddTO",
                controller: "app.views.employeeObject.addTO as vm",
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
            employeeObjectService.delete(id).then(function (results) {
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