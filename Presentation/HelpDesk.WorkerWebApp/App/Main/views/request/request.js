(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.request.request';
    app.controller(controllerId, [
        '$scope', '$rootScope', '$state', '$stateParams', 'employeeService', 'employeeObjectService', 'requestService', 'modalService', 'requestIdService',
        function ($scope, $rootScope, $state, $stateParams, employeeService, employeeObjectService, requestService, modalService, requestIdService) {

            var vm = this;
            vm.showAlert = true;

            vm.loadingFlag = false;
            $rootScope.$on('cfpLoadingBar:completed', function () {
                if (vm.loadingFlag) {
                    vm.loadingFlag = false;
                }
            });

            vm.request = {Id: 0};
            vm.errors = {};
            vm.newRequestId = 0;


            vm.invalidForm = function () {
                return !(vm.request.ObjectId && vm.request.DescriptionProblem &&
                    vm.request.ObjectId != -1 && vm.request.ObjectId != -2 &&
                    vm.request.EmployeeId);
            };

            vm.createNewRequest = function ()
            {
                vm.errors = {};
                requestService.get(0).then(function (results) {
                    vm.newRequestId = 0;
                    vm.request = results.data.data;
                    angular.element("#tempRequestKey").val(vm.request.TempRequestKey);
                    
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

                requestIdService.set(0);
                $scope.$broadcast('createNewRequestEvent');
                
            }

            
            vm.save = function () {
                vm.loadingFlag = true;
                requestService.save(vm.request).then(function (results) {
                    vm.errors = {};
                    vm.showAlert = true;
                    if (results.data.success === false) {
                        vm.errors = results.data.errors;
                    }
                    else {
                        vm.newRequestId = results.data.requestId;
                        $scope.$broadcast("editModeEvent", { editMode: false });
                    }
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };

            vm.edit = function ()
            {
                requestService.get(vm.newRequestId).then(function (results) {
                    vm.newRequestId = 0;
                    vm.request = results.data.data;
                    angular.element("#tempRequestKey").val(vm.request.TempRequestKey);
                    $scope.$broadcast("editModeEvent", { editMode: true });

                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });
            }

            vm.delete = function () {
                requestService.delete(vm.newRequestId).then(function (results) {
                    vm.errors = {};
                    vm.showAlert = true;
                    if (results.data.success === false) {
                        vm.errors = results.data.errors;
                    }
                    else {
                        vm.createNewRequest();
                    }

                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });
            }
            
            var init = function () {

                if ($stateParams.requestId) {

                    if ($stateParams.mode == 'create') {
                        requestService.getNewByRequestId($stateParams.requestId).then(function (results) {
                            vm.request = results.data.data;

                            angular.element("#tempRequestKey").val(vm.request.TempRequestKey);
                        }, function (error) {
                            $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                        });
                    }
                    else if ($stateParams.mode == 'edit') {
                        
                        requestIdService.set($stateParams.requestId);
                        requestService.get($stateParams.requestId).then(function (results) {
                            vm.request = results.data.data;
                            angular.element("#tempRequestKey").val(vm.request.TempRequestKey);
                            angular.element("#forignKeyId").val(vm.request.Id);
                        }, function (error) {
                            $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                        });
                    } 

                }
                else
                {
                    vm.createNewRequest();
                }

                
                
                vm.errors = {};
            }


            vm.openEmployeeObjectDictionary = function () {

                $rootScope.treeRequestParams = { employeeId: vm.request.EmployeeId };
                modalService.showModal({
                    templateUrl: "/AngularTemplate/EmployeeObjectTree",
                    controller: "app.views.object.list as vm"
                }).then(function (modal) {
                    modal.element.modal();
                    modal.close.then(function (result) {

                        if (result.cancel)
                            return;
                        vm.request.ObjectId = result.selectedObject.Id;
                        vm.request.ObjectName = result.selectedObject.Name;

                    });
                });
            }

            vm.getListEmployeeObject = function (name, response) {

                if (name == 'undefined' || name == 'null' || name == null)
                    return;

                employeeObjectService.getListEmployeeObjectByName(name, vm.request.EmployeeId).then(function (results) {
                    response($.map(results.data.data, function (t) {
                        return { label: t.ObjectName, value: t.ObjectName, item: t };
                    }));

                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };
          
            vm.selectEmployeeObject = function (p) {
                vm.request.ObjectId = p.ObjectId;
            };
               
            


            vm.openEmployeeDictionary = function () {
                modalService.showModal({
                    templateUrl: "/AngularTemplate/EmployeeTree",
                    controller: "app.views.employee.list as vm",
                    inputs: {
                        params: {}
                    }
                }).then(function (modal) {
                    modal.element.modal();
                    modal.close.then(function (result) {

                        if (result.cancel)
                            return;
                        vm.request.EmployeeId = result.selectedEmployee.Id;
                        vm.request.EmployeeInfo = result.selectedEmployee.Name;

                    });
                });
            }

            vm.getListEmployee = function (name, response) {

                if (name == 'undefined' || name == 'null' || name == null)
                    return;

                employeeService.getListEmployeeByName(name).then(function (results) {
                    response($.map(results.data.data, function (t) {
                        return { label: t.EmployeeInfo, value: t.EmployeeInfo, item: t };
                    }));

                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };

            vm.selectEmployee = function (p) {
                vm.request.EmployeeId = p.Id;
            };

            vm.clearEmployee = function () {
                vm.request.EmployeeId = 0;
                vm.request.EmployeeInfo = null;
            };


            $scope.$on('fileUploadFailEvent', function (event, data) {

                vm.showAlert = true;
                if (!vm.errors.ErrorGeneralMessage)
                    vm.errors.ErrorGeneralMessage = { Errors: [] };

                vm.errors.ErrorGeneralMessage.Errors.push({ ErrorMessage: data.errorMsg });

            });


            vm.closeAlert = function ()
            {
                vm.errors = {};
                vm.showAlert = false;
            }

            vm.addIS = function () {
                modalService.showModal({
                    templateUrl: "/AngularTemplate/EmployeeObjectAddIS",
                    controller: "app.views.employeeObject.addIS as vm",
                    inputs: {
                        params: {EmployeeId: vm.request.EmployeeId}
                    }
                }).then(function (modal) {
                    modal.element.modal();
                    modal.close.then(function (result) {

                        if (result.cancel)
                            return;
                        
                    });
                });
            }

            vm.addTO = function () {
                modalService.showModal({
                    templateUrl: "/AngularTemplate/EmployeeObjectAddTO",
                    controller: "app.views.employeeObject.addTO as vm",
                    inputs: {
                        params: { EmployeeId: vm.request.EmployeeId }
                    }
                }).then(function (modal) {
                    modal.element.modal();
                    modal.close.then(function (result) {

                        if (result.cancel)
                            return;
                        
                    });
                });
            }

            vm.addEditEmployee = function () {
                modalService.showModal({
                    templateUrl: "/AngularTemplate/Employee",
                    controller: "app.views.employee.employee as vm",
                    inputs: {
                        params: { EmployeeId: vm.request.EmployeeId }
                    }
                }).then(function (modal) {
                    modal.element.modal();
                    modal.close.then(function (result) {

                        if (result.cancel)
                            return;
                        
                        vm.request.EmployeeId = result.employee.Id;
                        vm.request.EmployeeInfo = result.employee.EmployeeInfo;
                        alert(vm.request.EmployeeInfo);
                        
                    });
                });
            }

            init();

        }
    ]);
})();