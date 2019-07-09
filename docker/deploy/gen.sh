if [ -z $ServerIdentity]; then
    ServerIdentity="http://id.mindnote.com"
fi
if [ -z $ServerApi]; then
    ServerApi="http://api.mindnote.com"
fi
if [ -z $ServerHost]; then
    ServerHost="http://mindnote.com"
fi
if [ -z $ClientHost]; then
    ClientHost="http://client.mindnote.com"
fi

if [ -z $SecretServerHost]; then
    SecretServerHost="secret"
fi
if [ -z $SecretServerApi]; then
    SecretServerApi="secret"
fi
if [ -z $SecretClientHost]; then
    SecretClientHost="secret"
fi

echo "Encode secrets..."

EncodedSecretServerHost=$(echo -n $SecretServerHost | openssl dgst -binary -sha256 | base64)
EncodedSecretServerApi=$(echo -n $SecretServerApi | openssl dgst -binary -sha256 | base64)
EncodedSecretClientHost=$(echo -n $SecretClientHost | openssl dgst -binary -sha256 | base64)

echo "  server-api : $EncodedSecretServerApi"
echo "  server-host: $EncodedSecretServerHost"
echo "  client-host: $EncodedSecretClientHost"

echo "Copy template files..."

mkdir config
cp -r template/* config/

echo "Apply templates..."

sed -i "s|{ServerIdentity}|$ServerIdentity|g" config/*.json
sed -i "s|{ServerApi}|$ServerApi|g" config/*.json
sed -i "s|{ServerHost}|$ServerHost|g" config/*.json
sed -i "s|{ClientHost}|$ClientHost|g" config/*.json
sed -i "s|{ServerIdentity}|$(echo $ServerIdentity | cut -d "/" -f3)|g" config/*.conf
sed -i "s|{ServerApi}|$(echo $ServerApi | cut -d "/" -f3)|g" config/*.conf
sed -i "s|{ServerHost}|$(echo $ServerHost | cut -d "/" -f3)|g" config/*.conf
sed -i "s|{ClientHost}|$(echo $ClientHost | cut -d "/" -f3)|g" config/*.conf
sed -i "s|{SecretServerHost}|$SecretServerHost|g" config/*
sed -i "s|{SecretServerApi}|$SecretServerApi|g" config/*
sed -i "s|{SecretClientHost}|$SecretClientHost|g" config/*
sed -i "s|{EncodedSecretServerHost}|$EncodedSecretServerHost|g" config/*
sed -i "s|{EncodedSecretServerApi}|$EncodedSecretServerApi|g" config/*
sed -i "s|{EncodedSecretClientHost}|$EncodedSecretClientHost|g" config/*
