using Microsoft.AspNetCore.Builder;

namespace Career635.Infrastructure.DependencyInjection;

public static class SecurityExtensions
{
    public static HeaderPolicyCollection GetSecurityPolicy(bool isDevelopment)
    {
        var policy = new HeaderPolicyCollection()
            // 1. FIX: Change 'Deny' to 'SameOrigin' 
            // This allows the CV IFrame to work while still blocking external sites.
            .AddFrameOptionsSameOrigin() 
            
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .RemoveServerHeader()
            .AddContentSecurityPolicy(builder =>
            {
                // Default: block everything except our own domain
                builder.AddDefaultSrc().Self();

                // 2. FIX: Allow Browser Refresh (WebSockets) for dotnet watch
                var connectBuilder = builder.AddConnectSrc().Self();
             

                // Allow local scripts (Lucide, Tom-Select, EasyMDE)
                builder.AddScriptSrc()
                    .Self()
                    .UnsafeInline();

                // Allow local styles (Tailwind v4)
                builder.AddStyleSrc()
                    .Self()
                    .UnsafeInline();

                // Allow local images (Photos) and Data URIs
                builder.AddImgSrc()
                    .Self()
                    .Data();

                // 3. FIX: Allow PDF Previews inside Frames/Objects
                builder.AddFrameSrc().Self();
                builder.AddObjectSrc().Self(); 

                builder.AddFormAction().Self();
            });

     

        return policy;
    }
}