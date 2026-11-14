#!/bin/sh
# Writes the API address into appsettings.json when the container starts.
set -e

cat > /usr/share/nginx/html/appsettings.json <<EOF
{
  "ApiBaseUrl": "${API_BASE_URL}"
}
EOF

# Remove pre-compressed copies of the old file, so only the new one is served.
rm -f /usr/share/nginx/html/appsettings.json.br /usr/share/nginx/html/appsettings.json.gz

echo "Pschool: API address set to ${API_BASE_URL}"