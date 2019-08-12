if(-not [string]::IsNullOrEmpty($Env:UserDnsDomain)) {
            $Subject = "${Env:ComputerName}.${Env:UserDnsDomain}"
}
else {
            $Subject = $Env:ComputerName
}
$certificate = New-SelfSignedCertificate -DnsName $Subject -CertStoreLocation "cert:\LocalMachine\My"
$thumb = $certificate.GetCertHashString()
& netsh http add sslcert ipport=0.0.0.0:9000 certhash=$thumb appid=`{1b1e7a9d-1af7-4922-88b9-8220e09cc072`}
