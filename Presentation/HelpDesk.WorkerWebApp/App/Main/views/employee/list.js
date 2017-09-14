(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.employee.list';
    app.controller(controllerId, [
        '$scope', '$element', 'close',
        function ($scope, $element, close) {

            var vm = this;

            $scope.employees.on("select_node.jstree", function (e, data) {
                if (data.node.id.indexOf("A") == 0)
                    return;
                var parent = data.instance.get_node(data.node.parent);
                $element.modal('hide');
                close({ cancel: false, selectedEmployee: { Name: data.node.text + ", " + parent.text, Id: data.node.id } }, 300);
            });

            $scope.employees.on("before_open.jstree", function (e, data) {
                for (var i = 0; i< data.node.children.length; i++)
                {
                    var n = data.instance.get_node(data.node.children[i]);
                    var el = data.instance.get_node(data.node.children[i], true);
                    if (n.original.type == "employee")
                    {
                        el.css('font-weight', 'bold');
                        el.css('color', '#2F4F4F');
                        el.css('font-style', 'italic');
                    }
                        
                }
            });
                        
            vm.cancel = function () {
                $element.modal('hide');
                close({ cancel: true }, 300);
            };
        
        }
    ]);
})();