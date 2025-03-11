namespace net.Models;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class ResendCodeRequest
{
    public string Email { get; set; }
}

public class ConfirmCodeRequest
{
    public string Email { get; set; }
    public string Code { get; set; }
}

public class CallbackRequest
{
    public string Code { get; set; }
}

public class LoginResponse
{
    public bool Ok { get; set; }
    public LoginResponseData Response { get; set; }
}

public class LoginResponseData
{
    public string idToken { get; set; }
    public string accessToken { get; set; }
    public string refreshToken { get; set; }
}

public class AwsResponse
{
    public bool Ok { get; set; }
    public ResponseData Response { get; set; }
}

public class ResponseData
{
    public MetaData Metadata { get; set; }
    public CodeDeliveryDetails CodeDeliveryDetails { get; set; }
    public bool UserConfirmed { get; set; }
    public string UserSub { get; set; }
}

public class MetaData
{
    public int HttpStatusCode { get; set; }
    public string RequestId { get; set; }
    public int Attempts { get; set; }
    public int TotalRetryDelay { get; set; }
}

public class CodeDeliveryDetails
{
    public string AttributeName { get; set; }
    public string DeliveryMedium { get; set; }
    public string Destination { get; set; }
} 