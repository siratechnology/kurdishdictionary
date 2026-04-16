using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class DropMeaning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meaning",
                table: "Words");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Meaning",
                table: "Words",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10000,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10001,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کباب");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10002,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆڵمە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10003,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نیسکێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10004,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ترخێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10005,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10006,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مریشک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10007,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10008,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10009,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەنیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10010,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10011,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیماغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10012,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنگوین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10013,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مورەبا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10014,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هێلکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10015,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نانی تیرێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10016,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەموون");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10017,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کولێرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10018,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەلانە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10019,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شلکێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10020,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سێو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10021,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مۆز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10022,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پرتەقاڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10023,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10024,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ترێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10025,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنجیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10026,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10027,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شووتی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10028,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کاڵەک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10029,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گێلاس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10030,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەماتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10031,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەیار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10032,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پیاز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10033,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10034,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بیبەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10035,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی باینجان");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10036,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کولەکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10037,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەتاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10038,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەوزە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10039,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەرەوز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10040,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەنجەڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10041,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قاپ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10042,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەوچک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10043,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چەتاڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10044,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چەقۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10045,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەرداغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10046,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆلکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10047,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سفرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10048,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەباغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10049,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فڕن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10050,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەلاجە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10051,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەڵاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10052,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هاوەن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10053,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەبەتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10054,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سینای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10055,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فنجان");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10056,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قۆری");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10057,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەماوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10058,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10059,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قاوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10060,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەکر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10061,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خوێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10062,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سماق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10063,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەردەچەوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10064,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دارچین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10065,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مێخەک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10066,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەیت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10067,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاوی تەماتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10068,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاوی لیمۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10069,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیرکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10070,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گوێز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10071,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بایەم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10072,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فستق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10073,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بندق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10074,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مێوژ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10075,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ناووکەوڕەقە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10076,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خورما");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10077,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گەنمی کوتراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10078,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10079,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نیسک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10080,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی یاپراخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10081,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کووپە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10082,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی جەوژەن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10083,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتەبڕاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10084,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قوراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10085,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیرەمۆز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10086,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10087,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەربەت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10088,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10089,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10090,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەحین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10091,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پاقڵاوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10092,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لوقمەقازی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10093,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بامیە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10094,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10095,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شفتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10096,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شیشکەباب");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10097,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پڵاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10098,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ساوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10099,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەلەم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10100,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پیزا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10101,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بەرگر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10102,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فینگەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10103,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سایندویچ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10104,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فەلافل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10105,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گەس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10106,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مایۆنێز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10107,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەتچەپ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10108,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەردەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10109,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10110,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سپێناخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10111,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەعدەنوس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10112,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10113,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەرۆزی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10114,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سڵق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10115,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ڕێواس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10116,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قارچک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10117,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کوندەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10118,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لازانیا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10119,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماکەرۆنی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10120,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆدڵز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10121,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10122,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شێلم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10123,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10124,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لۆبیا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10125,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆکی ڕەش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10126,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بڕوێش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10127,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گوێزاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10128,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاوی میوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10129,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاوی هەنار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10130,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتەی برنج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10131,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتەی ساوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10132,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شفتەی مریشک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10133,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی تەڕ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10134,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنجیری تەڕ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10135,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زالۆکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10136,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بابا غەنوج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10137,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی حومس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10138,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی موتەبەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10139,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فتووش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10140,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەبولە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10141,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتەی وەرزی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10142,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتەی پەتاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10143,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست و خەیار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10144,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست و موسیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10145,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆخیوا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10146,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گیپە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10147,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەروپێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10148,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10149,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مریشکی برژاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10150,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماسی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10151,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماسی مەسگوف");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10152,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتی بەرخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10153,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتی مانگا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10154,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەلیە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10155,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای نیسک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10156,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای جۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10157,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای سەوزە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10158,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی کوردی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10159,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی بسمەتی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10160,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی ڕەش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10161,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کێکی سادە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10162,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کێکی شوکولاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10163,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پسکیتی چای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10164,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی وشككراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10165,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەمر هیندی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10166,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەمەرەدین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10167,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی جەلی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10168,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەهەلەبی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10169,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10170,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کباب");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10171,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆڵمە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10172,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نیسکێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10173,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ترخێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10174,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10175,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مریشک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10176,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10177,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10178,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەنیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10179,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10180,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیماغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10181,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنگوین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10182,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مورەبا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10183,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هێلکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10184,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نانی تیرێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10185,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەموون");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10186,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کولێرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10187,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەلانە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10188,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شلکێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10189,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سێو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10190,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مۆز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10191,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پرتەقاڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10192,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10193,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ترێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10194,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنجیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10195,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10196,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شووتی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10197,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کاڵەک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10198,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گێلاس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10199,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەماتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10200,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەیار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10201,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پیاز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10202,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10203,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بیبەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10204,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی باینجان");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10205,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کولەکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10206,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەتاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10207,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەوزە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10208,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەرەوز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10209,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەنجەڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10210,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قاپ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10211,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەوچک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10212,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چەتاڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10213,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چەقۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10214,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەرداغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10215,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆلکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10216,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سفرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10217,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەباغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10218,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فڕن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10219,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەلاجە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10220,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەڵاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10221,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هاوەن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10222,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەبەتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10223,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سینای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10224,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فنجان");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10225,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قۆری");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10226,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەماوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10227,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10228,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قاوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10229,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەکر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10230,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خوێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10231,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سماق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10232,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەردەچەوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10233,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دارچین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10234,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مێخەک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10235,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەیت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10236,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاوی تەماتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10237,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاوی لیمۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10238,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیرکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10239,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گوێز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10240,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بایەم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10241,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فستق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10242,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بندق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10243,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مێوژ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10244,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ناووکەوڕەقە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10245,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خورما");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10246,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گەنمی کوتراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10247,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10248,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نیسک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10249,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی یاپراخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10250,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کووپە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10251,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی جەوژەن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10252,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتەبڕاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10253,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قوراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10254,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیرەمۆز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10255,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10256,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەربەت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10257,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10258,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10259,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەحین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10260,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پاقڵاوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10261,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لوقمەقازی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10262,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بامیە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10263,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10264,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شفتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10265,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شیشکەباب");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10266,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پڵاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10267,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ساوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10268,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەلەم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10269,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پیزا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10270,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بەرگر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10271,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فینگەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10272,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سایندویچ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10273,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فەلافل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10274,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گەس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10275,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مایۆنێز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10276,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەتچەپ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10277,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەردەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10278,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10279,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سپێناخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10280,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەعدەنوس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10281,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10282,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەرۆزی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10283,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سڵق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10284,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ڕێواس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10285,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قارچک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10286,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کوندەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10287,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لازانیا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10288,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماکەرۆنی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10289,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆدڵز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10290,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10291,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شێلم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10292,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10293,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لۆبیا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10294,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆکی ڕەش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10295,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بڕوێش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10296,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گوێزاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10297,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاوی میوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10298,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاوی هەنار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10299,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتەی برنج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10300,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتەی ساوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10301,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شفتەی مریشک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10302,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی تەڕ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10303,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنجیری تەڕ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10304,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زالۆکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10305,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بابا غەنوج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10306,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی حومس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10307,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی موتەبەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10308,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فتووش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10309,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەبولە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10310,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتەی وەرزی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10311,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتەی پەتاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10312,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست و خەیار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10313,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست و موسیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10314,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆخیوا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10315,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گیپە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10316,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەروپێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10317,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10318,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مریشکی برژاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10319,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماسی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10320,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماسی مەسگوف");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10321,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتی بەرخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10322,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتی مانگا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10323,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەلیە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10324,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای نیسک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10325,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای جۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10326,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای سەوزە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10327,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی کوردی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10328,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی بسمەتی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10329,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی ڕەش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10330,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کێکی سادە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10331,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کێکی شوکولاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10332,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پسکیتی چای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10333,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی وشككراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10334,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەمر هیندی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10335,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەمەرەدین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10336,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی جەلی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10337,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەهەلەبی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10338,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10339,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کباب");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10340,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆڵمە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10341,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نیسکێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10342,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ترخێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10343,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10344,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مریشک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10345,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10346,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10347,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەنیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10348,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10349,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیماغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10350,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنگوین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10351,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مورەبا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10352,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هێلکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10353,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نانی تیرێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10354,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەموون");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10355,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کولێرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10356,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەلانە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10357,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شلکێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10358,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سێو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10359,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مۆز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10360,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پرتەقاڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10361,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10362,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ترێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10363,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنجیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10364,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10365,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شووتی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10366,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کاڵەک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10367,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گێلاس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10368,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەماتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10369,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەیار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10370,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پیاز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10371,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10372,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بیبەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10373,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی باینجان");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10374,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کولەکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10375,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەتاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10376,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەوزە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10377,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەرەوز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10378,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەنجەڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10379,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قاپ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10380,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەوچک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10381,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چەتاڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10382,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چەقۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10383,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەرداغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10384,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆلکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10385,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سفرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10386,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەباغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10387,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فڕن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10388,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەلاجە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10389,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەڵاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10390,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هاوەن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10391,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەبەتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10392,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سینای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10393,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فنجان");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10394,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قۆری");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10395,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەماوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10396,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10397,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قاوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10398,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەکر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10399,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خوێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10400,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سماق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10401,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەردەچەوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10402,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دارچین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10403,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مێخەک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10404,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەیت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10405,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاوی تەماتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10406,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاوی لیمۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10407,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیرکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10408,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گوێز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10409,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بایەم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10410,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فستق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10411,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بندق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10412,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مێوژ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10413,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ناووکەوڕەقە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10414,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خورما");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10415,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گەنمی کوتراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10416,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10417,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نیسک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10418,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی یاپراخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10419,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کووپە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10420,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی جەوژەن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10421,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتەبڕاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10422,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قوراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10423,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیرەمۆز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10424,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10425,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەربەت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10426,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10427,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10428,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەحین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10429,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پاقڵاوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10430,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لوقمەقازی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10431,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بامیە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10432,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10433,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شفتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10434,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شیشکەباب");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10435,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پڵاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10436,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ساوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10437,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەلەم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10438,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پیزا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10439,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بەرگر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10440,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فینگەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10441,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سایندویچ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10442,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فەلافل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10443,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گەس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10444,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مایۆنێز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10445,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەتچەپ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10446,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەردەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10447,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10448,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سپێناخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10449,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەعدەنوس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10450,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10451,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەرۆزی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10452,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سڵق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10453,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ڕێواس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10454,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قارچک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10455,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کوندەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10456,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لازانیا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10457,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماکەرۆنی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10458,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆدڵز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10459,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10460,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شێلم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10461,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10462,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لۆبیا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10463,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆکی ڕەش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10464,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بڕوێش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10465,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گوێزاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10466,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاوی میوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10467,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاوی هەنار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10468,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتەی برنج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10469,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتەی ساوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10470,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شفتەی مریشک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10471,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی تەڕ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10472,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنجیری تەڕ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10473,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زالۆکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10474,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بابا غەنوج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10475,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی حومس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10476,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی موتەبەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10477,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فتووش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10478,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەبولە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10479,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتەی وەرزی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10480,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتەی پەتاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10481,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست و خەیار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10482,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست و موسیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10483,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆخیوا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10484,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گیپە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10485,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەروپێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10486,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10487,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مریشکی برژاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10488,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماسی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10489,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماسی مەسگوف");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10490,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتی بەرخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10491,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتی مانگا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10492,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەلیە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10493,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای نیسک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10494,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای جۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10495,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای سەوزە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10496,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی کوردی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10497,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی بسمەتی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10498,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی ڕەش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10499,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کێکی سادە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10500,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کێکی شوکولاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10501,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پسکیتی چای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10502,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی وشككراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10503,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەمر هیندی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10504,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەمەرەدین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10505,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی جەلی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10506,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەهەلەبی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10507,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10508,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کباب");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10509,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆڵمە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10510,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نیسکێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10511,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ترخێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10512,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10513,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مریشک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10514,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10515,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10516,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەنیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10517,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10518,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیماغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10519,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنگوین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10520,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مورەبا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10521,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هێلکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10522,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نانی تیرێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10523,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەموون");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10524,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کولێرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10525,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەلانە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10526,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شلکێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10527,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سێو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10528,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مۆز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10529,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پرتەقاڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10530,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10531,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ترێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10532,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنجیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10533,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10534,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شووتی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10535,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کاڵەک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10536,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گێلاس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10537,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەماتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10538,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەیار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10539,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پیاز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10540,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10541,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بیبەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10542,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی باینجان");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10543,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کولەکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10544,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەتاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10545,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەوزە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10546,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەرەوز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10547,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەنجەڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10548,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قاپ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10549,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەوچک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10550,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چەتاڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10551,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چەقۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10552,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەرداغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10553,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆلکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10554,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سفرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10555,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەباغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10556,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فڕن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10557,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەلاجە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10558,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەڵاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10559,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هاوەن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10560,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەبەتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10561,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سینای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10562,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فنجان");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10563,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قۆری");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10564,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەماوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10565,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10566,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قاوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10567,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەکر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10568,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خوێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10569,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سماق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10570,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەردەچەوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10571,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دارچین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10572,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مێخەک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10573,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەیت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10574,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاوی تەماتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10575,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاوی لیمۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10576,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیرکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10577,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گوێز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10578,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بایەم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10579,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فستق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10580,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بندق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10581,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مێوژ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10582,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ناووکەوڕەقە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10583,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خورما");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10584,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گەنمی کوتراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10585,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10586,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نیسک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10587,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی یاپراخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10588,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کووپە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10589,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی جەوژەن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10590,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتەبڕاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10591,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قوراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10592,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیرەمۆز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10593,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10594,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەربەت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10595,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10596,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10597,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەحین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10598,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پاقڵاوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10599,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لوقمەقازی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10600,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بامیە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10601,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10602,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شفتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10603,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شیشکەباب");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10604,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پڵاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10605,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ساوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10606,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەلەم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10607,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پیزا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10608,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بەرگر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10609,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فینگەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10610,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سایندویچ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10611,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فەلافل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10612,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گەس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10613,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مایۆنێز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10614,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەتچەپ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10615,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەردەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10616,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10617,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سپێناخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10618,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەعدەنوس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10619,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10620,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەرۆزی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10621,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سڵق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10622,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ڕێواس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10623,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قارچک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10624,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کوندەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10625,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لازانیا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10626,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماکەرۆنی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10627,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆدڵز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10628,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10629,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شێلم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10630,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10631,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لۆبیا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10632,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆکی ڕەش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10633,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بڕوێش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10634,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گوێزاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10635,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاوی میوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10636,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاوی هەنار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10637,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتەی برنج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10638,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتەی ساوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10639,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شفتەی مریشک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10640,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی تەڕ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10641,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنجیری تەڕ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10642,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زالۆکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10643,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بابا غەنوج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10644,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی حومس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10645,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی موتەبەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10646,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فتووش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10647,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەبولە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10648,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتەی وەرزی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10649,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتەی پەتاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10650,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست و خەیار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10651,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست و موسیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10652,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆخیوا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10653,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گیپە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10654,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەروپێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10655,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10656,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مریشکی برژاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10657,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماسی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10658,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماسی مەسگوف");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10659,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتی بەرخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10660,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتی مانگا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10661,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەلیە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10662,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای نیسک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10663,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای جۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10664,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای سەوزە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10665,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی کوردی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10666,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی بسمەتی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10667,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی ڕەش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10668,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کێکی سادە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10669,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کێکی شوکولاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10670,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پسکیتی چای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10671,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی وشككراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10672,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەمر هیندی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10673,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەمەرەدین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10674,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی جەلی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10675,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەهەلەبی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10676,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10677,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کباب");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10678,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆڵمە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10679,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نیسکێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10680,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ترخێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10681,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10682,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مریشک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10683,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10684,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10685,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەنیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10686,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10687,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیماغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10688,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنگوین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10689,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مورەبا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10690,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هێلکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10691,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نانی تیرێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10692,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەموون");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10693,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کولێرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10694,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەلانە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10695,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شلکێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10696,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سێو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10697,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مۆز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10698,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پرتەقاڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10699,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10700,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ترێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10701,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنجیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10702,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10703,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شووتی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10704,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کاڵەک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10705,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گێلاس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10706,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەماتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10707,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەیار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10708,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پیاز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10709,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10710,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بیبەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10711,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی باینجان");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10712,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کولەکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10713,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەتاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10714,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەوزە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10715,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەرەوز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10716,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەنجەڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10717,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قاپ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10718,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەوچک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10719,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چەتاڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10720,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چەقۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10721,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەرداغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10722,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆلکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10723,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سفرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10724,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەباغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10725,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فڕن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10726,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەلاجە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10727,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەڵاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10728,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هاوەن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10729,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەبەتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10730,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سینای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10731,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فنجان");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10732,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قۆری");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10733,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەماوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10734,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10735,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قاوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10736,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەکر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10737,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خوێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10738,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سماق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10739,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەردەچەوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10740,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دارچین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10741,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مێخەک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10742,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەیت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10743,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاوی تەماتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10744,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاوی لیمۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10745,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیرکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10746,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گوێز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10747,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بایەم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10748,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فستق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10749,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بندق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10750,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مێوژ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10751,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ناووکەوڕەقە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10752,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خورما");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10753,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گەنمی کوتراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10754,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10755,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نیسک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10756,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی یاپراخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10757,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کووپە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10758,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی جەوژەن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10759,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتەبڕاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10760,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قوراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10761,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیرەمۆز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10762,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10763,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەربەت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10764,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10765,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10766,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەحین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10767,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پاقڵاوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10768,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لوقمەقازی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10769,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بامیە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10770,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10771,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شفتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10772,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شیشکەباب");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10773,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پڵاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10774,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ساوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10775,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەلەم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10776,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پیزا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10777,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بەرگر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10778,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فینگەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10779,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سایندویچ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10780,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فەلافل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10781,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گەس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10782,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مایۆنێز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10783,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەتچەپ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10784,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەردەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10785,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10786,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سپێناخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10787,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەعدەنوس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10788,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10789,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەرۆزی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10790,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سڵق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10791,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ڕێواس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10792,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قارچک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10793,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کوندەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10794,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لازانیا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10795,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماکەرۆنی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10796,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆدڵز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10797,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10798,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شێلم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10799,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10800,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لۆبیا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10801,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆکی ڕەش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10802,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بڕوێش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10803,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گوێزاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10804,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاوی میوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10805,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاوی هەنار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10806,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتەی برنج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10807,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتەی ساوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10808,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شفتەی مریشک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10809,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی تەڕ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10810,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنجیری تەڕ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10811,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زالۆکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10812,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بابا غەنوج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10813,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی حومس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10814,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی موتەبەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10815,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فتووش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10816,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەبولە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10817,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتەی وەرزی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10818,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتەی پەتاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10819,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست و خەیار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10820,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست و موسیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10821,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆخیوا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10822,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گیپە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10823,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەروپێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10824,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10825,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مریشکی برژاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10826,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماسی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10827,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماسی مەسگوف");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10828,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتی بەرخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10829,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتی مانگا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10830,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەلیە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10831,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای نیسک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10832,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای جۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10833,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای سەوزە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10834,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی کوردی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10835,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی بسمەتی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10836,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنجی ڕەش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10837,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کێکی سادە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10838,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کێکی شوکولاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10839,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پسکیتی چای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10840,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی وشككراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10841,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەمر هیندی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10842,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەمەرەدین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10843,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی جەلی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10844,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەهەلەبی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10845,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی برنج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10846,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کباب");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10847,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆڵمە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10848,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نیسکێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10849,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ترخێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10850,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10851,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مریشک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10852,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10853,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10854,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەنیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10855,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10856,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیماغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10857,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنگوین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10858,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مورەبا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10859,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هێلکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10860,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نانی تیرێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10861,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەموون");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10862,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کولێرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10863,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەلانە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10864,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شلکێنە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10865,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سێو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10866,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مۆز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10867,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پرتەقاڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10868,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10869,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ترێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10870,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنجیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10871,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10872,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شووتی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10873,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کاڵەک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10874,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گێلاس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10875,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەماتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10876,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەیار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10877,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پیاز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10878,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10879,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بیبەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10880,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی باینجان");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10881,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کولەکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10882,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەتاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10883,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەوزە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10884,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەرەوز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10885,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەنجەڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10886,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قاپ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10887,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەوچک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10888,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چەتاڵ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10889,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چەقۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10890,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پەرداغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10891,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆلکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10892,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سفرە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10893,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەباغ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10894,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فڕن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10895,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەلاجە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10896,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەڵاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10897,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هاوەن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10898,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەبەتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10899,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سینای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10900,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فنجان");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10901,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قۆری");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10902,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەماوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10903,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی چای");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10904,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قاوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10905,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەکر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10906,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خوێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10907,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سماق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10908,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەردەچەوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10909,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دارچین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10910,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مێخەک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10911,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەیت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10912,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاوی تەماتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10913,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاوی لیمۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10914,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیرکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10915,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گوێز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10916,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بایەم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10917,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فستق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10918,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بندق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10919,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مێوژ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10920,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ناووکەوڕەقە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10921,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خورما");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10922,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گەنمی کوتراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10923,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10924,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نیسک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10925,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی یاپراخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10926,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کووپە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10927,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی جەوژەن");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10928,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتەبڕاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10929,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قوراو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10930,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سیرەمۆز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10931,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10932,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەربەت");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10933,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10934,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10935,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەحین");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10936,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پاقڵاوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10937,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لوقمەقازی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10938,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بامیە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10939,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10940,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شفتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10941,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شیشکەباب");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10942,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پڵاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10943,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ساوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10944,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شەلەم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10945,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی پیزا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10946,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بەرگر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10947,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فینگەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10948,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سایندویچ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10949,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فەلافل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10950,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گەس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10951,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مایۆنێز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10952,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەتچەپ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10953,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی خەردەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10954,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10955,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سپێناخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10956,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مەعدەنوس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10957,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کەوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10958,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەرۆزی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10959,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سڵق");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10960,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ڕێواس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10961,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قارچک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10962,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کوندەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10963,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لازانیا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10964,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماکەرۆنی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10965,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆدڵز");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10966,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10967,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شێلم");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10968,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10969,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی لۆبیا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10970,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی نۆکی ڕەش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10971,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بڕوێش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10972,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گوێزاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10973,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ئاوی میوە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10974,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆشاوی هەنار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10975,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتەی برنج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10976,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی کفتەی ساوەر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10977,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شفتەی مریشک");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10978,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەیسی تەڕ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10979,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی هەنجیری تەڕ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10980,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زالۆکە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10981,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی بابا غەنوج");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10982,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی حومس");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10983,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی موتەبەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10984,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی فتووش");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10985,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی تەبولە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10986,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتەی وەرزی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10987,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی زەڵاتەی پەتاتە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10988,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست و خەیار");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10989,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماست و موسیر");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10990,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی دۆخیوا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10991,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گیپە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10992,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی سەروپێ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10993,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەل");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10994,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی مریشکی برژاو");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10995,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماسی");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10996,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی ماسی مەسگوف");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10997,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتی بەرخ");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10998,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی گۆشتی مانگا");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10999,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی قەلیە");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 11000,
                column: "Meaning",
                value: "زانیاری ورد دەربارەی شۆربای نیسک");
        }
    }
}
