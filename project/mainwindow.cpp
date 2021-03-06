#include "mainwindow.h"
#include "aboutwindow.h"
#include "configwindow.h"
#include "ui_mainwindow.h"
#include "pathOperator.h"
#include "FileOperator.h"
#include "commonHeaders.h"
#include <QFileDialog>
#include <QDialog>
#include <QMovie>
#include <QLabel>
#include <QThread>
#include <QUrl>
#include <QPushButton>
#include <QHBoxLayout>
#include <QDesktopServices>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    setWindowTitle("WinFocus");
    setWindowIcon(QIcon(":/icon.ico"));
    setFixedSize(320,180);

    this->qGif_pressed=new QMovie(":/myBtn_light.gif");
    this->qGif_pressed->setScaledSize(QSize(60,60));
    this->qGif_released=new QMovie(":/myBtn_dark.gif");
    this->qGif_released->setScaledSize(QSize(60,60));


    ui->apply_btn->setStyleSheet("border-image: url(:/myBtn.png)");
    ui->apply_btn->setWindowFlags(Qt::WindowStaysOnTopHint);
    ui->apply_lbl->close();


    btn_list={"border-image: url(:/btn/btn_1.png)","border-image: url(:/btn/btn_2.png)","border-image: url(:/btn/btn_3.png)",
              "border-image: url(:/btn/btn_3.png)","border-image: url(:/btn/btn_4.png)","border-image: url(:/btn/btn_6.png)",
              "border-image: url(:/btn/btn_5.png)","border-image: url(:/btn/btn_6.png)","border-image: url(:/btn/btn_9.png)"};

    QObject::connect(ui->actionAbout,&QAction::triggered,this,&MainWindow::about_clicked);
    QObject::connect(ui->actionConfig,&QAction::triggered,this,&MainWindow::config_clicked);


    cfg=new Config("Config.ini");
    loct=new QString("NULL");

    QVariant var=cfg->Get("Config","Location");
    if(var.isValid())
    {
        *loct = var.value<QString>();
        qDebug()<<*loct;
        QLabel* label =new QLabel(this);
        label->setFrameStyle(QFrame::Box|QFrame::Sunken);
        label->setStyleSheet("font-size:10px");
        label->setStyleSheet("color:black");
        label->setStyleSheet("font-style:italic");
        label->setText("????????????????????????");
        ui->statusbar->addPermanentWidget(label);
    }
    else
    {
        ui->statusbar->setStyleSheet("font-size:10px");
        ui->statusbar->setStyleSheet("color:red");
        ui->statusbar->showMessage(tr("????????????????????????"));
    }
}

MainWindow::~MainWindow()
{
    delete ui;
}


void MainWindow::updateLocatState()
{
    cfg=new Config("Config.ini");
    QVariant var=cfg->Get("Config","Location");
    if(var.isValid())
    {
        *loct = var.value<QString>();
        qDebug()<<*loct;
    }
}

void MainWindow::about_clicked()
{
    aboutWindow *aboutWin =new aboutWindow();
    qDebug()<<"about Menubar-menu clicked";
    aboutWin->show();
}


void MainWindow::config_clicked()
{
    configWindow *configWin =new configWindow();
    qDebug()<<"config Menubar-menu clicked";
    configWin->show();
}


void MainWindow::on_openFolder_clicked()
{
    qDebug()<<"openFolder_Btn pressed";
    updateLocatState();
    if(*this->loct==QString::fromLocal8Bit("NULL"))
    {
        qDebug()<<"Location uninitialized.";
        ui->statusbar->setStyleSheet("font-size:11px");
        ui->statusbar->setStyleSheet("color:red");
        ui->statusbar->showMessage(tr("????????????????????????"));
        return;
    }
    //Load FileStoreLocation
    qDebug()<<"Open Folder clicked";
    QDesktopServices::openUrl(QUrl::fromLocalFile(*loct));
}


void MainWindow::on_apply_btn_clicked()
{
    qDebug()<<"apply_Btn clicked";
    updateLocatState();
    qDebug()<<*this->loct;
    if(*this->loct==QString::fromLocal8Bit("NULL"))
    {
        qDebug()<<"Location uninitialized.";
        ui->statusbar->setStyleSheet("font-size:10px");
        ui->statusbar->setStyleSheet("color:red");
        ui->statusbar->showMessage(tr("????????????????????????"));
        return;
    }
    QLabel* label =new QLabel(this);
    label->setFrameStyle(QFrame::Box|QFrame::Sunken);
    label->setStyleSheet("font-size:10px");
    label->setStyleSheet("color:black");
    label->setStyleSheet("font-style:italic");
    label->setText("????????????????????????");
    ui->statusbar->addPermanentWidget(label);

    ui->apply_btn->setEnabled(true);
    ui->openFolder->setEnabled(true);
    ui->apply_lbl->show();
    for(int i=0;i<=8;i++)
    {
        ui->apply_lbl->setStyleSheet(this->btn_list[i]);
        QThread::msleep(125);
    }

    getPicPath();
    QVector<QString> filenamelist;
    if(getPicName(prefix,filenamelist))
    {
        qDebug()<<"getPicNameList success!";
    }

    qDebug()<<"++++++++++++++++picNames+++++++++++++++++";

    for(int i=0;i<filenamelist.size();i++)
    {
        qDebug()<<filenamelist[i];
    }

    qDebug()<<"++++++++++++++++picNames+++++++++++++++++";

    ui->statusbar->setStyleSheet("font-size:11px;color:green");

    if(storeIni(filenamelist,*loct))
    {
        ui->statusbar->showMessage(tr("???????????????????????????"),200000);
        qDebug()<<"store FileName Success!";
        ui->apply_btn->setStyleSheet("border-image: url(:/btn/btn_9.png)");
        return;
    }
    ui->statusbar->showMessage(tr("???????????????????????????"),200000);
    ui->apply_btn->setStyleSheet("border-image: url(:/btn/btn_0.png)");
}

