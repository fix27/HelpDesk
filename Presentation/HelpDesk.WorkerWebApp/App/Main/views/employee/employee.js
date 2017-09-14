(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.employee.employee';
    app.controller(controllerId, [
        '$element', '$rootScope', 'employeeService', 'modalService', 'params',
        function ($element, $rootScope, employeeService, modalService, params) {

            var vm = this;

            vm.employee = {};
            vm.errors = {};
             
            vm.loadingFlag = false;
            $rootScope.$on('cfpLoadingBar:completed', function () {
                if (vm.loadingFlag) {
                    vm.loadingFlag = false;
                }
            });

            vm.invalidForm = function () {
                return !(vm.employee.FM && vm.employee.IM && vm.employee.OT);
            };

            
            vm.save = function () {
                vm.loadingFlag = true;
                employeeService.save(vm.employee).then(function (results) {
                    
                    vm.errors = {};
                    
                    if (results.data.success === false) {
                        vm.errors = results.data.errors;
                    }
                    else {
                        $element.modal('hide');
                        close({ cancel: false }, 300);
                    }
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };

            var employee = {};
            var init = function () {
                employeeService.get(params.EmployeeId).then(function (results) {
                    vm.employee = results.data.data;
                    
                    if (vm.employee.OrganizationName && vm.employee.OrganizationName != 'null')
                        vm.employee.OrganizationName += (", " + vm.employee.OrganizationAddress);

                    angular.copy(vm.employee, employee);
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

                vm.errors = {};
            }

            vm.hasChange = function ()
            {
                return !angular.equals(employee, vm.employee);
            }

            vm.getListPost = function (name, response) {

                if (name == 'undefined' || name == 'null' || name == null)
                    return;

                employeeService.getListPost(name).then(function (results) {
                    response($.map(results.data.data, function (t) {
                        return { label: t.Name, value: t.Name, item: t };
                    }));

                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };
            
            vm.selectPost = function (p) {
                vm.employee.PostId = p.Id;
            };

            vm.getListOrganization = function (name, response) {

                if (name == 'undefined' || name == 'null' || name == null)
                    return;

                employeeService.getListOrganization(name).then(function (results) {
                    response($.map(results.data.data, function (t) {
                        return { label: t.Name + ", " + t.Address, value: t.Name + ", " + t.Address, item: t };
                    }));

                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };

            vm.selectOrganization = function (o) {
                vm.employee.OrganizationId = o.Id;
            };

            vm.openOrganizationDictionary = function ()
            {
                modalService.showModal({
                    templateUrl: "/AngularTemplate/OrganizationTree",
                    controller: "app.views.organization.list as vm",
                    inputs: {
                        params: { }
                    }
                }).then(function (modal) {
                    modal.element.modal();
                    modal.close.then(function (result) {

                        if (result.cancel)
                            return;
                        vm.employee.OrganizationId = result.organization.Id;
                        vm.employee.OrganizationName = result.organization.Name;

                    });
                });
            }
                        
            vm.cancel = function () {
                $element.modal('hide');
                close({ cancel: true }, 300);
            };

            init();
     
        }
    ]);
})();