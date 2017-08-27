(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.requestProfile.addIS';
    app.controller(controllerId, [
        '$rootScope', '$element', 'close','requestProfileService',
        function ($rootScope, $element, close, requestProfileService) {

            var vm = this;

            vm.requestObjectIS = {};
            vm.errors = {};
             
            

            vm.invalidForm = function () {
                return !(vm.requestObjectIS.Id);
            };

            
            vm.save = function () {
                                
                requestProfileService.addIS(vm.requestObjectIS).then(function (results) {
                    
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
                requestProfileService.getNewRequestObjectIS().then(function (results) {
                    vm.requestObjectIS = results.data.data;
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

                requestProfileService.getListAllowableObjectIS().then(function (results) {
                    vm.requestObjects = results.data.data;
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });
                                
                vm.errors = {};
            }

            //для автокомплит поля
            vm.getListAllowableObjectIS = function (name, response) {

                if (name == 'undefined' || name == 'null' || name == null)
                    return;

                requestProfileService.getListAllowableObjectIS(name).then(function (results) {
                    response($.map(results.data.data, function (t) {
                        return { label: t.ObjectName, value: t.ObjectName, item: t };
                    }));

                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };
            
            vm.selectAllowableObjectIS = function (o) {
                vm.requestObjectIS.Id = o.Id;
            };

                        
            init();
     
        }
    ]);
})();