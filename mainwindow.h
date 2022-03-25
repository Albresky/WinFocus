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
    Ui::MainWindow *ui;

private slots:
    void about_clicked();

    void config_clicked();

    void on_apply_btn_pressed();

    void on_apply_btn_released();

    void on_openFolder_clicked();

private:
    QMovie* qGif_pressed;
    QMovie* qGif_released;
    QStringList btn_list;
    Config* cfg;
    QString* loct;

    void updateLocatState();
};
#endif // MAINWINDOW_H
