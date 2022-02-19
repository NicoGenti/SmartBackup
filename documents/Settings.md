# /Config/appsettings.json


  - 1 If you download the Zip, extract the Zip archive anywhere you want on your PC;

  - 2 navigate into subfolder *Config* and open *appsettings.json* with text editor  for settings;

  - 3 set *Folder variables*;
    
    ```json
    "FolderSettings": {
        "gitPath": "git",
        "pathFileOrg": "/Config/ListOrganization.txt",
        "rootBackup": "/MASTER_FOLDER_FOR_ZIP_BACKUP_NAME",
        "nameBackup": "Backup"
      }
    ```
    
  - 4 set Git variables;
    
    ```json
    "GitSettings": {
        "gitUser": "USERNAME",
        "gitToken": "YOUR_GIT_TOKEN_REPO"
      }
    ```
    
  - 5 set Email variables;
    
    ```json
    "EmailSettings": {
        "EmailSubject": "EMAIL_SUBJECT",
        "FromEmail": "EMAIL_SENDER",
        "ToEmail": "EMAIL_RECIVER",
        "FromName": "NAME_RECIVER",
        "ApiKeySendgrid": "iNSERT_API_KKEY_SENDGRID"
      }
    ```

