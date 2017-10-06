(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.organization.list';
    app.controller(controllerId, [
        '$scope', '$element', 'close',
        function ($scope, $element, close) {

            var vm = this;

            $scope.organizations.on("select_node.jstree", function (e, data) {

                var n = data.instance.get_node(data.node);
                if (!n.original.selectable)
                    return;

                $element.modal('hide');
                close({ cancel: false, organization: { Name: data.node.text, Id: data.node.id } }, 300);
            });

            $scope.organizations.on("load_node.jstree", function (e, data) {
                var n = data.instance.get_node(data.node);
                var el = data.instance.get_node(data.node, true);
                if (n.original && !n.original.selectable)
                {
                    var t = el.find('.jstree-anchor:first');
                    t.css('color', '#AEB6BF');
                }
               
                for (var i = 0; i < data.node.children.length; i++) {
                    n = data.instance.get_node(data.node.children[i]);
                    el = data.instance.get_node(data.node.children[i], true);
                    if (!n.original.selectable)
                        el.css('color', '#AEB6BF');
                }
                
            });
                        
            vm.cancel = function () {
                $element.modal('hide');
                close({ cancel: true }, 300);
            };
        
        }
    ]);
})();