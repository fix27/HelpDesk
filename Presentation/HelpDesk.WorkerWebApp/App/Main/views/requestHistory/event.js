(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.requestHistory.event';
    app.controller(controllerId, ['$rootScope', '$element', 'params', 'close', 'requestService',
        function ($rootScope, $element, params, close, requestService) {

            var vm = this;
            vm.showAlert = true;
            vm.errors = {};

            vm.requestEvent = {};
            vm.requestEvent.RequestId = params.requestId;
            vm.requestEvent.StatusName = params.statusName;
            vm.requestEvent.StatusRequestId = params.statusRequestId;
            vm.requestEvent.Note = null;
            vm.requestEvent.NewDeadLineDate = null;
            vm.requestEvent.MaxDeadLineDate = null;

            if (vm.requestEvent.StatusRequestId == HelpDesk.WorkerWebApp.Resources.ExtendedDeadLine)
            {
                requestService.getAllowableDeadLine().then(function (results) {
                    vm.requestEvent.NewDeadLineDate = new Date(Date.parse(results.data.data.Value1));
                    vm.requestEvent.MaxDeadLineDate = new Date(Date.parse(results.data.data.Value2));
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });
            }

            vm.invalidForm = function ()
            {
                return false;
            }

            vm.save = function ()
            {
                requestService.createRequestEvent(vm.requestEvent).then(function (results) {
                    vm.errors = {};
                    vm.showAlert = true;
                    if (results.data.success === false) {
                        vm.errors = results.data.errors;
                    }
                    else {
                        $element.modal('hide');
                        close({ cancel: false }, 300);
                    }
                }, function (error) {
                    $element.modal('hide');
                    close({ cancel: true }, 300);
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });
            }

            vm.cancel = function () {
                vm.showAlert = true;
                $element.modal('hide');
                close({ cancel: true }, 300);
            }

            vm.closeAlert = function () {
                vm.errors = {};
                vm.showAlert = false;
            }
        }

    ]);
})();