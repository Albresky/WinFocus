#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "picOperator.h"
#include "commonHeaders.h"
#include "config.h"

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void about_clicked();

    void config_clicked();

    void on_apply_btn_pressed();

    void on_apply_btn_released();

//    void on_apply_btn_clicked();

    void on_pushButton_clicked();

private:
    Ui::MainWindow *ui;
    QMovie* qGif_pressed;
    QMovie* qGif_released;
    QStringList btn_list;
    Config* cfg;
    QString* loct;
};
#endif // MAINWINDOW_H
