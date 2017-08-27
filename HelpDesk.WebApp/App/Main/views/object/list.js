(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.object.list';
    app.controller(controllerId, [
        '$scope', '$element', 'close',
        function ($scope, $element, close) {

            var vm = this;

            $scope.objects.on("select_node.jstree", function (e, data) {
                if (data.node.id == -1 || data.node.id == -2)
                    return;

                $element.modal('hide');
                close({ cancel: false, personalObject: { Name: data.node.text, Id: data.node.id } }, 300);
            });
                        
            vm.cancel = function () {
                $element.modal('hide');
                close({ cancel: true }, 300);
            };
        
        }
    ]);
})();