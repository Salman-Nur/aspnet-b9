
using Amazon.S3;
using Amazon.S3.Model;

AmazonS3Client client = new AmazonS3Client();

PutBucketRequest request = new PutBucketRequest
{
    BucketName = "aspnetb9test",
    UseClientRegion = true
};

string path = "C://folder";
string filename = Guid.NewGuid().ToString();

PutObjectRequest r1 = new PutObjectRequest
{
    Key = filename,
    BucketName = "aspnetb9"
};



PutBucketResponse response = await client.PutBucketAsync(request);

if(response != null && response.HttpStatusCode == System.Net.HttpStatusCode.OK)
{
    foreach(var item in response.ResponseMetadata.Metadata)
        Console.WriteLine($"Key: {item.Key}, Value:{item.Value}");
}