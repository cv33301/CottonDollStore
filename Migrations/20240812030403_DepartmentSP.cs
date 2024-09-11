using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbCottonDollStore.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banner",
                columns: table => new
                {
                    BID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerImg = table.Column<string>(type: "char(30)", unicode: false, fixedLength: true, maxLength: 30, nullable: false),
                    Information = table.Column<string>(type: "char(200)", unicode: false, fixedLength: true, maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner", x => x.BID);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ParentCategory = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__19093A2B841B10F7", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK__Category__Parent__3D5E1FD2",
                        column: x => x.ParentCategory,
                        principalTable: "Category",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateTable(
                name: "Rank",
                columns: table => new
                {
                    RankID = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    RankName = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rank__B37AFB960001A619", x => x.RankID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: false),
                    UserPhone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UserImg = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Account = table.Column<string>(type: "char(14)", unicode: false, fixedLength: true, maxLength: 14, nullable: true),
                    RankID = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    RegDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    StoreID = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__1788CCACCBF52B00", x => x.UserID);
                    table.ForeignKey(
                        name: "FK__User__RankID__47DBAE45",
                        column: x => x.RankID,
                        principalTable: "Rank",
                        principalColumn: "RankID");
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    StoreID = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    UserID = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    StoreInformation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Store__3B82F0E1AAA21DE9", x => x.StoreID);
                    table.ForeignKey(
                        name: "FK__Store__UserID__4AB81AF0",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<string>(type: "char(14)", unicode: false, fixedLength: true, maxLength: 14, nullable: false, defaultValueSql: "EXEC getOrderID"),
                    StoreID = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    UserID = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Payment = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Logistics = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ConNumber = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    PreDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShipDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PickupDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Statu = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__C3905BAF352A2270", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK__Order__StoreID__25518C17",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Order__UserID__2645B050",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProID = table.Column<string>(type: "char(9)", unicode: false, fixedLength: true, maxLength: 9, nullable: false),
                    ProName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Information = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProImg = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CategoryID = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Clicks = table.Column<int>(type: "int", nullable: true),
                    CategoryID_2 = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    CategoryID_3 = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    Statu = table.Column<bool>(type: "bit", nullable: true),
                    StoreID = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__620295F09BB96820", x => x.ProID);
                    table.ForeignKey(
                        name: "FK__Product__Categor__02084FDA",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK__Product__Categor__02FC7413",
                        column: x => x.CategoryID_2,
                        principalTable: "Category",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK__Product__Categor__03F0984C",
                        column: x => x.CategoryID_3,
                        principalTable: "Category",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK__Product__StoreID__01142BA1",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "ProductSpec",
                columns: table => new
                {
                    ProSpecID = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    ProID = table.Column<string>(type: "char(9)", unicode: false, fixedLength: true, maxLength: 9, nullable: false),
                    StoreID = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    SpecImg = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Spec = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(6,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductS__5C04AED6164834B6", x => new { x.ProSpecID, x.ProID, x.StoreID });
                    table.ForeignKey(
                        name: "FK__ProductSp__ProID__398D8EEE",
                        column: x => x.ProID,
                        principalTable: "Product",
                        principalColumn: "ProID");
                    table.ForeignKey(
                        name: "FK__ProductSp__Store__571DF1D5",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderID = table.Column<string>(type: "char(14)", unicode: false, fixedLength: true, maxLength: 14, nullable: false),
                    ProSpecID = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    ProID = table.Column<string>(type: "char(9)", unicode: false, fixedLength: true, maxLength: 9, nullable: false),
                    StoreID = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    BuyerReview = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    SellerRespond = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    Star = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__473213D7FDDD3F54", x => new { x.OrderID, x.ProSpecID, x.ProID });
                    table.ForeignKey(
                        name: "FK__OrderDeta__Order__2739D489",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "FK__OrderDeta__ProID__17F790F9",
                        column: x => x.ProID,
                        principalTable: "Product",
                        principalColumn: "ProID");
                    table.ForeignKey(
                        name: "FK__OrderDeta__Store__19DFD96B",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID");
                    table.ForeignKey(
                        name: "FK__OrderDetail__1DB06A4F",
                        columns: x => new { x.ProSpecID, x.ProID, x.StoreID },
                        principalTable: "ProductSpec",
                        principalColumns: new[] { "ProSpecID", "ProID", "StoreID" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategory",
                table: "Category",
                column: "ParentCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Order_StoreID",
                table: "Order",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserID",
                table: "Order",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProID",
                table: "OrderDetail",
                column: "ProID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProSpecID_ProID_StoreID",
                table: "OrderDetail",
                columns: new[] { "ProSpecID", "ProID", "StoreID" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_StoreID",
                table: "OrderDetail",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryID",
                table: "Product",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryID_2",
                table: "Product",
                column: "CategoryID_2");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryID_3",
                table: "Product",
                column: "CategoryID_3");

            migrationBuilder.CreateIndex(
                name: "IX_Product_StoreID",
                table: "Product",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpec_ProID",
                table: "ProductSpec",
                column: "ProID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpec_StoreID",
                table: "ProductSpec",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Store_UserID",
                table: "Store",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_User_RankID",
                table: "User",
                column: "RankID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banner");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "ProductSpec");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Rank");
        }
    }
}
