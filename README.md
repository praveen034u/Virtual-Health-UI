Blazor WebAssembly frontend consuming APIs, showing dashboards, medication reminders, AI insights, and insurance information.

# Using AWS CLI (preferred for CI/CD)

# Create bucket first-
aws s3api create-bucket \
  --bucket your-blazor-ui-bucket-name \
  --region us-west-2 \
  --create-bucket-configuration LocationConstraint=us-west-2

#Enable Static Website Hosting:

aws s3 website s3://your-blazor-ui-bucket-name/ \
  --index-document index.html \
  --error-document index.html

#Set Public Read Access Policy (Optional for public UI)
#Enable CORS for API Calls
#Save this as cors-config.json: Apply it:
aws s3api put-bucket-policy \
  --bucket your-blazor-ui-bucket-name \
  --policy file://bucket-policy.json

#Step 5: IAM Permissions for GitHub Actions
#Your GitHub Actions workflow will use these credentials.

#Create an IAM user with Programmatic access.

#Attach this inline policy or a managed policy like AmazonS3FullAccess (or scoped version):
  {
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": ["s3:PutObject", "s3:DeleteObject", "s3:ListBucket"],
      "Resource": [
        "arn:aws:s3:::your-blazor-ui-bucket-name",
        "arn:aws:s3:::your-blazor-ui-bucket-name/*"
      ]
    }
  ]
}
