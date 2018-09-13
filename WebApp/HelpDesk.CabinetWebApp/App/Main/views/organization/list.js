(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.organization.list';
    app.controller(controllerId, [
        '$scope', '$element', 'close',
        function ($scope, $element, close) {

            var vm = this;

            $scope.organizations.on("select_node.jstree", function (e, data) {

                $element.modal('hide');
                close({ cancel: false, organization: { Name: data.node.text, Id: data.node.id } }, 300);
            });
                        
            vm.cancel = function () {
                $element.modal('hide');
                close({ cancel: true }, 300);
            };
        
        }
    ]);
})();