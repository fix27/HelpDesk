using System.Web.Optimization;

namespace HelpDesk.WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            //VENDOR RESOURCES

            //~/Bundles/App/vendor/css
            bundles.Add(
                new StyleBundle("~/Bundles/App/vendor/css")
                    .Include(
                        "~/Content/bootstrap.css",
                        "~/Content/jquery-ui.css",
                        "~/Content/loading-bar.css"
                    )
                );
            
            //~/Bundles/App/vendor/js
            bundles.Add(
                new ScriptBundle("~/Bundles/App/vendor/js")
                    .Include(
                        
                        "~/Scripts/json2.min.js",

                        "~/Scripts/modernizr-2.8.3.js",

                        "~/Scripts/jquery-2.1.4.min.js",
                        "~/Scripts/jquery-ui-1.11.4.min.js",

                        "~/Scripts/bootstrap.min.js",

                        "~/Scripts/moment-with-locales.min.js",
                        "~/Scripts/jquery.blockUI.js",
                        "~/Scripts/toastr.min.js",
                        "~/Scripts/sweetalert/sweet-alert.min.js",
                        "~/Scripts/others/spinjs/spin.js",
                        "~/Scripts/others/spinjs/jquery.spin.js",

                        
                        "~/Scripts/angular.min.js",
                        "~/Scripts/angular-route.js",
                        "~/Scripts/angular-aria.js",
                        "~/Scripts/angular-messages.js",
                        "~/Scripts/angular-animate.min.js",
                        "~/Scripts/svg-assets-cache.js",
                        "~/Scripts/angular-sanitize.min.js",
                        "~/Scripts/angular-ui-router.min.js",
                        "~/Scripts/angular-ui/ui-bootstrap.js",
                        "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                        "~/Scripts/angular-ui/ui-utils.js",
                        "~/Scripts/i18n/angular-locale_ru-ru.js",
                        "~/Scripts/loading-bar.js",
                        "~/Scripts/respond.js"
                    )
                );
           

              //~/Bundles/App/Main/css
              bundles.Add(
                new StyleBundle("~/Bundles/App/Main/css")
                    .IncludeDirectory("~/App/Main", "*.css", true)
                );

            
            //~/Bundles/App/Main/js
            bundles.Add(
                new ScriptBundle("~/Bundles/App/Main/js")
                    .IncludeDirectory("~/App/Main", "*.js")
                    .IncludeDirectory("~/App/Main/views/layout", "*.js")
                    .IncludeDirectory("~/App/Main/views/shared", "*.js")
                    .IncludeDirectory("~/App/Main/views/personalProfile", "*.js")
                    .IncludeDirectory("~/App/Main/views/requestProfile", "*.js")
                    .IncludeDirectory("~/App/Main/views/request", "*.js")
                    .IncludeDirectory("~/App/Main/views/requestHistory", "*.js")
                    .IncludeDirectory("~/App/Main/views/organization", "*.js")
                    .IncludeDirectory("~/App/Main/views/object", "*.js")
                    .IncludeDirectory("~/App/Main/services", "*.js")
                    .IncludeDirectory("~/App/Main/directives", "*.js")
                );


            bundles.Add(new StyleBundle("~/Content/jQuery-File-Upload").Include(
                    "~/Content/jQuery.FileUpload/css/jquery.fileupload.css",
                   "~/Content/jQuery.FileUpload/css/jquery.fileupload-ui.css",
                   "~/Content/blueimp-gallery2/css/blueimp-gallery.css",
                     "~/Content/blueimp-gallery2/css/blueimp-gallery-video.css",
                       "~/Content/blueimp-gallery2/css/blueimp-gallery-indicator.css"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/jQuery-File-Upload").Include(
                //<!-- The Templates plugin is included to render the upload/download listings -->
                "~/Scripts/jQuery.FileUpload/vendor/jquery.ui.widget.js",
                //"~/Scripts/jQuery.FileUpload/tmpl.min.js",
                //<!-- The Load Image plugin is included for the preview images and image resizing functionality -->
                "~/Scripts/jQuery.FileUpload/load-image.all.min.js",
                //<!-- The Canvas to Blob plugin is included for image resizing functionality -->
                "~/Scripts/jQuery.FileUpload/canvas-to-blob.min.js",
                //"~/Scripts/file-upload/jquery.blueimp-gallery.min.js",
                //<!-- The Iframe Transport is required for browsers without support for XHR file uploads -->
                "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
                //<!-- The basic File Upload plugin -->
                "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                //<!-- The File Upload processing plugin -->
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-process.js",
                //<!-- The File Upload image preview & resize plugin -->
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-image.js",
                //<!-- The File Upload audio preview plugin -->
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-audio.js",
                //<!-- The File Upload video preview plugin -->
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-video.js",
                //<!-- The File Upload validation plugin -->
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-validate.js",
                //!-- The File Upload user interface plugin -->
                //"~/Scripts/jQuery.FileUpload/jquery.fileupload-ui.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-angular.js",
                //Blueimp Gallery 2 
                "~/Scripts/blueimp-gallery2/js/blueimp-gallery.js",
                "~/Scripts/blueimp-gallery2/js/blueimp-gallery-video.js",
                "~/Scripts/blueimp-gallery2/js/blueimp-gallery-indicator.js",
                "~/Scripts/blueimp-gallery2/js/jquery.blueimp-gallery.js"

));
        }
    }
}
