mc alias set local http://minio:9000 $MINIO_ROOT_USER $MINIO_ROOT_PASSWORD

mc mb local/project-tracker-reports || true

mc anonymous set download local/project-tracker-reports || true

mc admin user add local appuser apppassword || true

mc admin policy create local appolicy ./scripts/minio-policy.json || true

mc admin policy attach local appolicy --user appuser

mc admin user svcacct add local appuser \
  --access-key appaccesskey \
  --secret-key appsecretkey || true