You may have noticed that there are references to a file **Secrets** in **namespace CodeCapture.Models** at various instances. That file has not been added to the repository as it contains the following sensitive information:

* Gmail Address
* Gmail Password
* Azure Computer Vision Read API Subscription Key
* Azure Computer Vision Read API Endpoint
* JDoodle Compiler API Client ID
* JDoodle Compiler API Client Secret

You can create your own version of the **Secrets.cs** in this [folder](https://github.com/adityaoberai/CodeCapture/tree/master/CodeCapture/CodeCapture/Models) in the following format:

```

using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCapture.Models
{
    class Secrets
    {
        public string EMAIL_address { get; set; } = <Gmail Address>
        public string EMAIL_password { get; set; } = <Gmail Password>;
        public string READ_subscriptionKey { get; set; } = <Azure Computer Vision Read API Subscription Key>;
        public string READ_endpoint { get; set; } = <Azure Computer Vision Read API Endpoint>;
        public string JDOODLE_clientId { get; set; } = <JDoodle Compiler API Client ID>;
        public string JDOODLE_clientSecret { get; set; } = <JDoodle Compiler API Client Secret>;
    }
}

```
