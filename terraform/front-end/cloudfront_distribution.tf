

resource "aws_cloudfront_distribution" "cloudfront_distribution" {
	comment         = "piggybank-${var.environment_name}"
	enabled         = true
	is_ipv6_enabled = true

	default_root_object = "index.html"
	aliases             = [ local.hostname ]

	origin {
		domain_name = aws_s3_bucket.s3_bucket.website_endpoint
		origin_id   = aws_s3_bucket.s3_bucket.bucket

		custom_origin_config {
			http_port              = "80"
			https_port             = "443"
			origin_protocol_policy = "http-only"
			origin_ssl_protocols   = [ "TLSv1.2" ]
		}
	}

	custom_error_response {
		error_code            = 404
		response_page_path    = "/index.html"
		response_code         = 200
		error_caching_min_ttl = 3600
	}

	custom_error_response {
		error_code            = 403
		response_page_path    = "/index.html"
		response_code         = 200
		error_caching_min_ttl = 3600
	}

	default_cache_behavior {
		allowed_methods  = [ "GET", "HEAD" ]
		cached_methods   = [ "GET", "HEAD" ]
		target_origin_id = aws_s3_bucket.s3_bucket.bucket

		forwarded_values {
			query_string = false
			cookies {
				forward = "none"
			}
		}

		viewer_protocol_policy = "redirect-to-https"
	}

	restrictions {
		geo_restriction {
			restriction_type = "none"
		}
	}

	viewer_certificate {
		acm_certificate_arn = aws_acm_certificate.certificate.arn
		ssl_support_method  = "sni-only"
	}
}