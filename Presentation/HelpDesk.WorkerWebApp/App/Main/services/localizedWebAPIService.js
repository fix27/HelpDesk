(function () {
    var app = angular.module('app');
    app.factory('localizedWebAPIService', [
        function () {

            var factory = {};

            var _get = function (uri) {
                //culture - глобальная переменная, находится в \App\Main\views\layout\layout.cshtml
                //устанавливается на стороне сервера в Razor
                return "/api/" + culture + "/" + uri;
            };
                       
            
            factory.get = _get;
            
            return factory;

        }
    ]);
})();