#!/bin/bash

# 1. Initialize npm if package.json doesn't exist
if [ ! -f package.json ]; then
    echo "Initializing npm..."
    npm init -y
fi

# 2. Install Tailwind v4 CLI   "DefaultConnection": "Server=localhost,1433;Database=Career635DB;User Id=sa;Password=Abcd1234..;TrustServerCertificate=True;MultipleActiveResultSets=true",

echo "Installing Tailwind v4..."
npm install @tailwindcss/cli

# 3. Create folder structure
# mkdir -p src dist

# 4. Create the CSS entry point (v4 style)
# echo '@import "tailwindcss";' > src/app.css

# 5. Add scripts to package.json using Node
# (This avoids installing extra tools like jq)
node -e '
const fs = require("fs");
const pkg = JSON.parse(fs.readFileSync("package.json", "utf8"));
pkg.scripts = {
  ...pkg.scripts,
  "dev": "npx @tailwindcss/cli -i ./wwwroot/css/site.css -o  ./wwwroot/dist/style.css --watch",
  "build": "npx @tailwindcss/cli -i ./wwwroot/css/site.css  -o  ./wwwroot/dist/style.css --minify"
};
fs.writeFileSync("package.json", JSON.stringify(pkg, null, 2));
'

echo "--------------------------------------"
echo "Tailwind v4 Setup Complete!"
echo "1. To start developing: npm run dev"
echo "2. Link your HTML to:  ./wwwroot/dist/style.css"
echo "--------------------------------------"
