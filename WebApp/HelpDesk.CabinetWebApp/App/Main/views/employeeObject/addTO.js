﻿(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.employeeObject.addTO';
    app.controller(controllerId, [
        '$rootScope', '$element', 'close','employeeObjectService',
        function ($rootScope, $element, close, employeeObjectService) {

            var vm = this;

            vm.requestObjectTO = {};
            vm.errors = {};
             
            

            vm.invalidForm = function () {
                return !(vm.requestObjectTO.ObjectTypeId &&
                    vm.requestObjectTO.HardTypeName &&
                    vm.requestObjectTO.ManufacturerName &&
                    vm.requestObjectTO.ModelName);
            };

            
            vm.save = function () {
                                
                employeeObjectService.addTO(vm.requestObjectTO).then(function (results) {
                    
                    vm.errors = {};

                    if (results.data.success === false) {
                        vm.errors = results.data.errors;
                    }
                    else {
                        //закрытие диалога, обновление гриды на родительской форме
                        $element.modal('hide');
                        close({ cancel: false }, 300);
                    }
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };

            vm.cancel = function () {
                $element.modal('hide');
                close({ cancel: true }, 300);
            };

            var init = function () {
                employeeObjectService.getNewRequestObjectTO().then(function (results) {
                    vm.requestObjectTO = results.data.data;
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

                employeeObjectService.getListAllowableObjectType().then(function (results) {
                    vm.objectTypes = results.data.data;
                    if (vm.objectTypes)
                        vm.requestObjectTO.ObjectTypeId = vm.objectTypes[0].Id;
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });
                                
                vm.errors = {};
            }

            //для автокомплит полей
            vm.getListHardType = function (name, response) {

                if (name == 'undefined' || name == 'null' || name == null)
                    return;

                employeeObjectService.getListHardType(name).then(function (results) {
                    response($.map(results.data.data, function (t) {
                        return { label: t.Name, value: t.Name, item: t };
                    }));

                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };

            vm.selectHardType = function (o) {
                vm.requestObjectTO.HardTypeId = o.Id;
            };

            vm.getListManufacturer = function (name, response) {

                if (name == 'undefined' || name == 'null' || name == null)
                    return;

                employeeObjectService.getListManufacturer(name).then(function (results) {
                    response($.map(results.data.data, function (t) {
                        return { label: t.Name, value: t.Name, item: t };
                    }));

                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };

            vm.selectManufacturer = function (o) {
                vm.requestObjectTO.ManufacturerId = o.Id;
            };

            vm.getListModel = function (name, response) {

                if (!vm.requestObjectTO.ManufacturerId || name == 'undefined' || name == 'null' || name == null)
                    return;

                employeeObjectService.getListModel(vm.requestObjectTO.ManufacturerId, name).then(function (results) {
                    response($.map(results.data.data, function (t) {
                        return { label: t.Name, value: t.Name, item: t };
                    }));

                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };

            vm.selectModel = function (o) {
                vm.requestObjectTO.ModelId = o.Id;
            };
                                    
            init();
     
        }
    ]);
})();