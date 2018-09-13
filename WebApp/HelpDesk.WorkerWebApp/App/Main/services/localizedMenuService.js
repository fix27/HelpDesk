(function () {
    var app = angular.module('app');
    app.factory('localizedMenuService', [
        function () {

            var factory = {};

            var _get = function () {
                //menu - глобальная переменная, находится в \App\Main\views\layout\layout.cshtml
                //устанавливается на стороне сервера в Razor
                return menu;
            };
                       
            
            factory.get = _get;
            
            return factory;

        }
    ]);
})();