variable "mime_type_lookup" {
	type    = map
	default = {
		"html" = "text/html"
		"css"  = "text/css"
		"js"   = "application/javascript"
		"png"  = "image/png"
		"svg"  = "image/svg+xml"
	}
}

resource "aws_s3_bucket" "s3_bucket" {
	bucket = "piggybank-${var.environment_name}"
	acl    = "public-read"

	website {
		index_document = "index.html"
		error_document = "index.html"
	}
}

resource "aws_s3_bucket_object" "s3_bucket_objects" {
	for_each = fileset("${path.module}/../../src/pb-app/build", "**")

	bucket = aws_s3_bucket.s3_bucket.bucket
	acl    = "public-read"
	key    = each.value
	source = "${path.module}/../../src/pb-app/build/${each.value}"
	etag   = filemd5("${path.module}/../../src/pb-app/build/${each.value}")

	content_type = lookup(var.mime_type_lookup, split(".", each.value)[length(split(".", each.value)) - 1], "application/octet-stream")
}