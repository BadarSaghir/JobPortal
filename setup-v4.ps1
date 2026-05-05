# 1. Initialize npm
if (!(Test-Path "package.json")) { npm init -y }

# 2. Install Tailwind v4 (Alpha/Beta/Stable CLI)
npm install @tailwindcss/cli
#  "DefaultConnection": "Server=(local);Database=Career635DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;",

# 3. Create the CSS entry point (The v4 way)



# 4. Update package.json scripts
$packageJson = Get-Content "package.json" | ConvertFrom-Json
$packageJson.scripts | Add-Member -MemberType NoteProperty -Name "dev" -Value "npx @tailwindcss/cli -i ./wwwroot/css/site.css -o ./wwwroot/dist/style.css --watch" -Force
$packageJson.scripts | Add-Member -MemberType NoteProperty -Name "build" -Value "npx @tailwindcss/cli -i ./wwwroot/css/site.css -o ./wwwroot/dist/style.css --minify" -Force
$packageJson | ConvertTo-Json -Depth 10 | Set-Content "package.json"

Write-Host "Tailwind v4 Setup Complete!" -ForegroundColor Green
Write-Host "No config file needed in v4!" -ForegroundColor Magenta