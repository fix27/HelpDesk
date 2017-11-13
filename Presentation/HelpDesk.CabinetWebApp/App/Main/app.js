(function () {
    'use strict';
    
    var app = angular.module('app', [
        'ngAnimate',
        'ngSanitize',
        'ngRoute',
        'ui.router',
        'ui.bootstrap',
        'ui.jq',
        'ui.keypress',
        'ui.event',
        'autocomplete.directive',
        'jsTree.directive',
        'angular-loading-bar',
        'blueimp.fileupload'
    ]);

    //Configuration for Angular UI routing.
    //culture - глобальная переменная, определена в \App\Main\views\layout\layout.cshtml
    app.config([
        '$stateProvider', '$urlRouterProvider', '$httpProvider', 'fileUploadProvider',
        function ($stateProvider, $urlRouterProvider, $httpProvider, fileUploadProvider, $state) {
            
            $urlRouterProvider.otherwise('/');
            $stateProvider
                .state('request', {
                    url: '/',
                    templateUrl: '/' + culture + '/AngularTemplate/NewRequest',
                    menu: 'request',
                    params: { requestId: null, objectId: null, mode: null }

                })
                .state('requestHistory', {
                    url: '/',
                    templateUrl: '/' + culture + '/AngularTemplate/RequestHistory',
                    menu: 'requestHistory',

                })
                .state('employeeObject', {
                    url: '/',
                    templateUrl: '/' + culture + '/AngularTemplate/EmployeeObjects',
                    menu: 'employeeObject',

                })
                .state('employee', {
                    url: '/',
                    templateUrl: '/' + culture + '/AngularTemplate/Employee',
                    menu: 'employee'
                });

            delete $httpProvider.defaults.headers.common['X-Requested-With'];
            fileUploadProvider.defaults.redirect = window.location.href.replace(
                /\/[^\/]*$/,
                '/cors/result.html?%s'
            );


            

            // Demo settings:
            angular.extend(fileUploadProvider.defaults, {
                // Enable image resizing, except for Android and Opera,
                // which actually support image resizing, but fail to
                // send Blob objects via XHR requests:
                disableImageResize: /Android(?!.*Chrome)|Opera/
                    .test(window.navigator.userAgent),
                maxFileSize: 500000,
                maxNumberOfFiles: 5,
                minFileSize: 1,
                acceptFileTypes: /(\.|\/)(gif|jpeg|png|docx|xlsx|doc|xls)$/i
            });

        }
    ]);

    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push(function ($rootScope, $q) {
            return {
                request: function (config) {
                    config.timeout = 10000;
                    return config;
                },
                responseError: function (rejection) {
                    switch (rejection.status) {
                        case 408:
                            alert('connection timed out');
                            break;
                    }
                    return $q.reject(rejection);
                }
            }
        })
    })
     
})();