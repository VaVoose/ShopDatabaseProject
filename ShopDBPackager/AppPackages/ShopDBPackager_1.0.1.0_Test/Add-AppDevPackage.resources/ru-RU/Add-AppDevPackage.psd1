# Localized	02/24/2021 05:12 AM (GMT)	303:4.80.0411 	Add-AppDevPackage.psd1
# Culture = "en-US"
ConvertFrom-StringData @'
###PSLOC
PromptYesString=Д&а
PromptNoString=&Нет
BundleFound=Найденный набор: {0}
PackageFound=Обнаружен пакет: {0}
EncryptedBundleFound=Найден зашифрованный набор: {0}
EncryptedPackageFound=Найден зашифрованный пакет: {0}
CertificateFound=Обнаружен сертификат: {0}
DependenciesFound=Обнаружены пакеты зависимостей:
GettingDeveloperLicense=Получение лицензии разработчика...
InstallingCertificate=Установка сертификата...
InstallingPackage=\nУстановка приложения...
AcquireLicenseSuccessful=Лицензия разработчика успешно получена.
InstallCertificateSuccessful=Сертификат успешно установлен.
Success=\nУспех: приложение успешно установлено.
WarningInstallCert=\nВы собираетесь установить цифровой сертификат в хранилище сертификатов "Доверенные лица" своего компьютера. Эта операция связана с серьезными рисками для безопасности и должна выполняться, только если вы доверяете источнику данного цифрового сертификата.\n\nПосле завершения использования приложения вы должны будете вручную удалить данный цифровой сертификат. Соответствующие инструкции можно найти по следующему адресу: http://go.microsoft.com/fwlink/?LinkId=243053\n\nПродолжить?\n\n
ElevateActions=\nПеред установкой этого приложения необходимо выполнить следующие действия:
ElevateActionDevLicense=\t- Получить лицензию разработчика
ElevateActionCertificate=\t- Установить сертификат подписи
ElevateActionsContinue=Для продолжения требуются учетные данные разработчика.  Нажмите "ОК" в сообщении контроля учетных записей и укажите пароль администратора, если появится соответствующий запрос.
ErrorForceElevate=Для продолжения необходимо указать учетные данные администратора.  Запустите этот скрипт без параметра -Force или из окна PowerShell с повышенными правами.
ErrorForceDeveloperLicense=Получение лицензии разработчика предполагает участие пользователя.  Заново запустите этот скрипт без параметра -Force.
ErrorLaunchAdminFailed=Ошибка: не удалось запустить новый процесс от имени администратора.
ErrorNoScriptPath=Ошибка: необходимо запустить этот скрипт из файла.
ErrorNoPackageFound=Ошибка: в каталоге скрипта не найден пакет или набор.  Проверьте, что устанавливаемый пакет или набор находится в одном каталоге со скриптом.
ErrorManyPackagesFound=Ошибка: в каталоге скрипта найдено несколько пакетов или наборов.  Проверьте, что в каталоге со скриптом находится только устанавливаемый пакет или набор.
ErrorPackageUnsigned=Ошибка: пакет или набор не подписан цифровой подписью, либо подпись повреждена.
ErrorNoCertificateFound=Ошибка: в каталоге скрипта не найден сертификат.  Проверьте, что в каталоге со скриптом находится сертификат, используемый для подписывания устанавливаемого пакета или набора.
ErrorManyCertificatesFound=Ошибка: в каталоге скрипта найдено несколько сертификатов.  Проверьте, что в каталоге со скриптом находится только сертификат, используемый для подписывания устанавливаемого пакета или набора.
ErrorBadCertificate=Ошибка: файл "{0}" не является допустимым цифровым сертификатом.  Программа CertUtil вернула код ошибки {1}.
ErrorExpiredCertificate=Ошибка: срок действия сертификата разработчика {0} истек. Одной из возможных причин является неправильная настройка даты и времени в системных часах. Если системные параметры настроены правильно, обратитесь к владельцу приложения для повторного создания пакета или набора с действительным сертификатом.
ErrorCertificateMismatch=Ошибка: сертификат не совпадает с сертификатом, с помощью которого был подписан пакет или набор.
ErrorCertIsCA=Ошибка: сертификат не может быть центром сертификации.
ErrorBannedKeyUsage=Ошибка: в сертификате не может быть следующего значения назначения ключа: {0}.  Значение назначения ключа может быть не задано или равняться "DigitalSignature".
ErrorBannedEKU=Ошибка: в сертификате не может быть следующего расширенного назначения ключа: {0}.  Допускаются только расширенные назначения для подписывания кода и подписывания времени жизни.
ErrorNoBasicConstraints=Ошибка: в сертификате отсутствует расширение базовых ограничений.
ErrorNoCodeSigningEku=Ошибка: в сертификате отсутствует расширенное назначение ключа для подписывания кода.
ErrorInstallCertificateCancelled=Ошибка: установка сертификата была отменена.
ErrorCertUtilInstallFailed=Ошибка: не удалось установить сертификат.  Программа CertUtil вернула код ошибки {0}.
ErrorGetDeveloperLicenseFailed=Ошибка: не удалось получить лицензию разработчика. Дополнительные сведения см. по адресу http://go.microsoft.com/fwlink/?LinkID=252740.
ErrorInstallCertificateFailed=Ошибка: не удалось установить сертификат. Состояние: {0}. Дополнительные сведения см. по адресу http://go.microsoft.com/fwlink/?LinkID=252740.
ErrorAddPackageFailed=Ошибка: не удалось установить приложение.
ErrorAddPackageFailedWithCert=Ошибка: не удалось установить приложение.  Для обеспечения безопасности рекомендуется удалить сертификат подписи до времени установки приложения.  Соответствующие инструкции можно найти по следующему адресу:\nhttp://go.microsoft.com/fwlink/?LinkId=243053
'@
