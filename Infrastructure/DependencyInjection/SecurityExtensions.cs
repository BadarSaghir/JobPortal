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
                   .RemoveServerHeader()
            
            // 2. REMOVE X-Powered-By header (IIS/ASP.NET)
            .RemovePoweredByHeader()
            
            // 3. REMOVE X-AspNet-Version header
            .RemoveAspNetVersionHeaders()
            
            // 4. REMOVE X-AspNetMvc-Version header
            .RemoveMvcVersionHeader()
                  
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
        private static HeaderPolicyCollection RemovePoweredByHeader(this HeaderPolicyCollection policy)
    {
        policy.AddCustomHeader("X-Powered-By", "");
        return policy;
    }
    
    // Extension method to remove ASP.NET version headers
    private static HeaderPolicyCollection RemoveAspNetVersionHeaders(this HeaderPolicyCollection policy)
    {
        // This removes X-AspNet-Version header
        policy.AddCustomHeader("X-AspNet-Version", "");
        return policy;
    }
    
    // Extension method to remove MVC version header
    private static HeaderPolicyCollection RemoveMvcVersionHeader(this HeaderPolicyCollection policy)
    {
        // This removes X-AspNetMvc-Version header
        policy.AddCustomHeader("X-AspNetMvc-Version", "");
        return policy;
    }
}