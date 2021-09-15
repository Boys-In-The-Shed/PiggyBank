resource "aws_route53_record" "route53_record" {
	name    = local.hostname
	type    = "A"
	zone_id = data.aws_route53_zone.hosted_zone.id
	
	alias {
		name                   = aws_cloudfront_distribution.cloudfront_distribution.domain_name
		zone_id                = aws_cloudfront_distribution.cloudfront_distribution.hosted_zone_id
		evaluate_target_health = true
	}
}