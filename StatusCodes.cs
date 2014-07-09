
namespace ReportFromXenu
{
    enum ReportStatusCodes
    {
        Ok = 200,
        NoInfoToReturn = 204,
        NoObjectData = 400,
        AuthRequired = 401,
        ForbiddenRequest = 403,
        NotFound = 404,
        NoLongerAvailable = 410,
        ServerError = 500,
        Timeout = 12002,
        NoSuchHost = 12007,
        Cancelled = 12017,
        NoConnection = 12029,
        SkipType = -3,
        ExternalLinks = -2,
        Mailto = -6,
        AllUrls = 0,
        AllUrlsButOk = 1,
        WithoutExternalUrlsAndOk = 2,
        OnlyMediaUrls = 3,
        OnlyOldurls = 4,
        NotFoundInternalUrls = 5
    }
}
