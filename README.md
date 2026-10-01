====ВАРИАНТ 1. ПРОЕКТ ЗАБИРАЕМ С GITHUB ====

1.  ОБНОВИТЬ СПИСОК ПАКЕТОВ

sudo apt update

2.  УСТАНОВИТЬ .NET 8, NGINX И GIT

sudo apt install -y dotnet-sdk-8.0 nginx git

Проверка (необязательно):

dotnet –version git –version nginx -v

3.  СОЗДАТЬ ПАПКУ ДЛЯ ПРОЕКТА И ПЕРЕЙТИ В НЕЕ

mkdir -p /home/USER/appcd/home/USER/app

4.  СКАЧАТЬ ПРОЕКТ С GITHUB

git clone https://github.com/dreamzzz1/ExamApi.git .

ВАЖНО: точка в конце нужна, чтобы проект клонировался прямо в текущую
папку.

Проверить:

ls

Должны быть, среди прочего: Controllers Models deploy ExamApi.csproj
Program.cs

5.  СОЗДАТЬ ПАПКУ ДЛЯ РЕЛИЗА

sudo mkdir -p /var/www/app

6.  СОБРАТЬ И ОПУБЛИКОВАТЬ ПРОЕКТ

sudo dotnet publish -c Release --output /var/www/app

Проверить:

ls /var/www/app

Там должен быть ExamApi.dll.

7.  ВЫСТАВИТЬ ВЛАДЕЛЬЦА И ПРАВА

sudo chown -R www-data:www-data /var/www/app 
sudo chmod -R 755 /var/www/app

8.  СКОПИРОВАТЬ ГОТОВЫЙ SYSTEMD-КОНФИГ

sudo cp deploy/examapi.service /etc/systemd/system/examapi.service

9.  СКОПИРОВАТЬ ГОТОВЫЙ NGINX-КОНФИГ

sudo cp deploy/nginx-examapi /etc/nginx/sites-available/examapi

10. ВКЛЮЧИТЬ НАШ САЙТ В NGINX И УБРАТЬ СТАНДАРТНЫЙ

sudo ln -s /etc/nginx/sites-available/examapi /etc/nginx/sites-enabled/
sudo rm -f /etc/nginx/sites-enabled/default

11. ПРОВЕРИТЬ КОНФИГУРАЦИЮ NGINX

sudo nginx -t

Нормальный результат: syntax is ok test is successful

12. ПЕРЕЧИТАТЬ SYSTEMD И ЗАПУСТИТЬ API

sudo systemctl daemon-reload 
sudo systemctl enable --now examapi

13. ПРОВЕРИТЬ SYSTEMD-СЕРВИС

sudo systemctl status examapi

Нужно увидеть: Active: active (running)

Для выхода из просмотра нажать: q

14. ПЕРЕЗАПУСТИТЬ NGINX

sudo systemctl restart nginx

15. ПРОВЕРИТЬ API ВНУТРИ UBUNTU

curl http://localhost/health 
curl http://localhost/api/products 
curl http://localhost/api/categories

Ожидается примерно:

{“status”:“Healthy”}

[{“id”:1,“name”:“Laptop”,“price”:80000},{“id”:2,“name”:“Phone”,“price”:50000}]

[{“id”:1,“name”:“Electronics”},{“id”:2,“name”:“Accessories”}]

16. ПОКАЗАТЬ ИСТОРИЮ КОМАНД

history | tail -n 50

===== ВАРИАНТ 2. ПРОЕКТ БЕРЕМ С ФЛЕШКИ =====

На флешке должна лежать ВСЯ папка ExamApi. В ней обязательно должна быть
папка deploy с файлами: examapi.service nginx-examapi

1.  ОБНОВИТЬ ПАКЕТЫ

sudo apt update

2.  УСТАНОВИТЬ .NET 8 И NGINX

sudo apt install -y dotnet-sdk-8.0 nginx

3.  ВСТАВИТЬ ФЛЕШКУ И НАЙТИ ЕЕ

lsblk

Также посмотреть:

ls /media/$USER/

Например, если флешка называется USB, она может находиться здесь:

/media/$USER/USB

4.  СОЗДАТЬ ПАПКУ ДЛЯ ПРОЕКТА

mkdir -p /home/$USER/app

5.  СКОПИРОВАТЬ ПРОЕКТ С ФЛЕШКИ

ПРИМЕР (USB заменить на реальное имя флешки):

cp -R /media/USER/USB/ExamApi/./home/USER/app/

Перейти:

cd /home/$USER/app

Проверить:

ls

Должны быть: Controllers Models deploy ExamApi.csproj Program.cs

6.  ДАЛЬШЕ ВСЕ КАК В ВАРИАНТЕ С GITHUB

sudo mkdir -p /var/www/app

sudo dotnet publish -c Release –output /var/www/app

sudo chown -R www-data:www-data /var/www/app sudo chmod -R 755
/var/www/app

sudo cp deploy/examapi.service /etc/systemd/system/examapi.service sudo
cp deploy/nginx-examapi /etc/nginx/sites-available/examapi

sudo ln -s /etc/nginx/sites-available/examapi /etc/nginx/sites-enabled/
sudo rm -f /etc/nginx/sites-enabled/default

sudo nginx -t

sudo systemctl daemon-reload sudo systemctl enable –now examapi

sudo systemctl status examapi

Нажать q для выхода.

sudo systemctl restart nginx

curl http://localhost/health curl http://localhost/api/products curl
http://localhost/api/categories

history | tail -n 50

===== ПРОВЕРКА С WINDOWS =====

По заданию работу API нужно показать запросами с Windows.

1.  СНАЧАЛА УЗНАТЬ IP UBUNTU

В Ubuntu:

ip -brief address

Нужен IP сетевого интерфейса, доступного Windows. Например:

192.168.56.31

НЕ использовать: 127.0.0.1

Если VM работает только через VirtualBox NAT и имеет только адрес вроде:
10.0.2.15

Windows-хост может не иметь прямого доступа к VM. Тогда в VirtualBox
удобно добавить второй Host-Only адаптер. После этого снова выполнить:

ip -brief address

и взять появившийся адрес вида 192.168.56.x.

2.  ПРОВЕРИТЬ С WINDOWS

Открыть PowerShell или CMD на Windows.

Допустим, IP Ubuntu: 192.168.56.31

Тогда выполнить:

curl http://192.168.56.31/health

curl http://192.168.56.31/api/products

curl http://192.168.56.31/api/categories

ВАЖНО: использовать http://, НЕ https://

Если возвращается JSON — Nginx доступен с Windows и приложение работает.

===== ЧТО ЗДЕСЬ ПРОИСХОДИТ ======

ExamApi работает как systemd-сервис на:

localhost:5000

Nginx слушает:

порт 80

и передает запросы:

80 -> 5000

Поэтому с Windows мы обращаемся просто:

http://IP_UBUNTU/health

а не:

http://IP_UBUNTU:5000/health

==== ЕСЛИ ЧТО-ТО НЕ РАБОТАЕТ ====

Проверить API-сервис:

sudo systemctl status examapi

Посмотреть последние логи:

sudo journalctl -u examapi -n 50 –no-pager

Проверить Nginx:

sudo nginx -t sudo systemctl status nginx

Проверить, слушается ли порт 5000:

curl http://localhost:5000/health

Проверить через Nginx:

curl http://localhost/health
