git clone https://github.com/StardustDL/MindNote.git ./src

echo "Configure host names..."

if [ -z $BackendIdentity ]; then
    read -p "  Input backend identity: " BackendIdentity
fi
if [ -z $BackendApi ]; then
    read -p "  Input backend api: " BackendApi
fi
if [ -z $FrontendServer ]; then
    read -p "  Input frontend server: " FrontendServer
fi
if [ -z $FrontendClient ]; then
    read -p "  Input frontend client: " FrontendClient
fi

echo "Configure client secrets..."

if [ -z $SecretFrontendServer ]; then
    read -p "  Input frontend server: " SecretFrontendServer
fi
if [ -z $SecretBackendApi ]; then
    read -p "  Input backend api: " SecretBackendApi
fi
if [ -z $SecretFrontendClient ]; then
    read -p "  Input frontend client: " SecretFrontendClient
fi

cd src
git checkout dev
cd ..

cp -r ./src/docker/deploy/* ./deploy
cd deploy
chmod +x gen.sh
./gen.sh

echo "Starting..."

sudo docker-compose up -d