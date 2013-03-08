import urllib, urllib2
import httplib
import ssl
import socket
import os

CERT_FILE = os.path.join(os.path.dirname(__file__), 'cacert.pem')

cafile_local = 'cacert.pem'
cafile_remote = 'http://curl.haxx.se/ca/cacert.pem'
if not os.path.isfile(CERT_FILE):
    print >> sys.stderr, "Downloading %s from %s" % (
        CERT_FILE, cafile_remote)
    urllib.urlretrieve(cafile_remote, cafile_local)

class ValidHTTPSConnection(httplib.HTTPConnection):
        "This class allows communication via SSL."

        default_port = httplib.HTTPS_PORT

        def __init__(self, *args, **kwargs):
            httplib.HTTPConnection.__init__(self, *args, **kwargs)

        def connect(self):
            "Connect to a host on a given (SSL) port."

            sock = socket.create_connection((self.host, self.port),
                                            self.timeout, self.source_address)
            if self._tunnel_host:
                self.sock = sock
                self._tunnel()
            self.sock = ssl.wrap_socket(sock,
                                        ca_certs=CERT_FILE,
                                        cert_reqs=ssl.CERT_REQUIRED)


class ValidHTTPSHandler(urllib2.HTTPSHandler):

    def https_open(self, req):
            return self.do_open(ValidHTTPSConnection, req)

opener = urllib2.build_opener(ValidHTTPSHandler)


def test_access(url):
    print "Acessing", url
    page = opener.open(url)
    print page.info()
    data = page.read()
    print "First 100 bytes:", data[0:100]
    print "Done accesing", url
    print ""

# This should work
test_access("https://www.google.com")

# Accessing a page with a self signed certificate should not work
# At the time of writing, the following page uses a self signed certificate
try:
  test_access("https://tidia.ita.br/")
except urllib2.URLError as e:
  assert 'SSL3_GET_SERVER_CERTIFICATE' in str(e.reason)
  print "Done accesing with invalid SSL CERT!"
