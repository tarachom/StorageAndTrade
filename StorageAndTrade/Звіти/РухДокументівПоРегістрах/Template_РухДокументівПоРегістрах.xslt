<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	
    <xsl:output method="html" indent="yes"/>

	<xsl:template name="Head">
		<meta charset="utf-8" />
		<meta name="viewport" content="width=device-width, initial-scale=1" />
		<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css" />
		<script src="https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.slim.min.js"></script>
		<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
		<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.bundle.min.js"></script>
	</xsl:template>

	<xsl:template match="/root">

		<html>
			<head>
				<title>Table</title>
				<xsl:call-template name="Head" />
			</head>
			<body>

				<div class="container">
					<h2>Рух документу по регістрах</h2>

					<xsl:apply-templates select="ТовариНаСкладах" />

					<xsl:apply-templates select="РухТоварів" />

					<xsl:apply-templates select="ЗамовленняКлієнтів" />

					<xsl:apply-templates select="РозрахункиЗКлієнтами" />

					<xsl:apply-templates select="ВільніЗалишки" />

					<xsl:apply-templates select="ЗамовленняПостачальникам" />

					<xsl:apply-templates select="РозрахункиЗПостачальниками" />

					<xsl:apply-templates select="ТовариДоПоступлення" />

					<br/>
					<br/>
					<br/>
					<br/>

				</div>

			</body>
		</html>

	</xsl:template>
	
	<xsl:template match="ТовариНаСкладах">
		<br/>
		<h5>Товари на cкладах</h5>

		<table class="table table-bordered table-sm">
			<tr>
				<th>period</th>
				<th>income</th>
				<th>Номенклатура</th>
				<th>Характеристика номенклатури</th>
				<th>Склад</th>
				<th>ВНаявності</th>
				<th>ДоВідвантаження</th>
			</tr>

			<xsl:for-each select="row">
				<tr>
					<td>
						<xsl:value-of select="period"/>
					</td>
					<td>
						<xsl:value-of select="income"/>
					</td>
					<td>
						<xsl:value-of select="Номенклатура_Назва"/>
					</td>
					<td>
						<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Склад_Назва"/>
					</td>
					<td>
						<xsl:value-of select="ВНаявності"/>
					</td>
					<td>
						<xsl:value-of select="ДоВідвантаження"/>
					</td>
				</tr>
			</xsl:for-each>

		</table>

	</xsl:template>

	<xsl:template match="РухТоварів">
		<br/>
		<h5>Рух товарів</h5>

		<table class="table table-bordered table-sm">

			<tr>
				<th>period</th>
				<th>income</th>
				<th>Номенклатура</th>
				<th>Характеристика номенклатури</th>
				<th>Склад</th>
				<th>Кількість</th>
			</tr>

			<xsl:for-each select="row">

				<tr>
					<td>
						<xsl:value-of select="period"/>
					</td>
					<td>
						<xsl:value-of select="income"/>
					</td>
					<td>
						<xsl:value-of select="Номенклатура_Назва"/>
					</td>
					<td>
						<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Склад_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Кількість"/>
					</td>
				</tr>

			</xsl:for-each>

		</table>

	</xsl:template>
	
	<xsl:template match="ЗамовленняКлієнтів">
		<br/>
		<h5>Замовлення клієнтів</h5>
		
		<table class="table table-bordered table-sm">

			<tr>
				<th>period</th>
				<th>income</th>
				<th>ЗамовленняКлієнта</th>
				<th>Номенклатура</th>
				<th>Характеристика номенклатури</th>
				<th>Склад</th>
				<th>Замовлено</th>
				<th>Сума</th>
			</tr>

			<xsl:for-each select="row">

				<tr>
					<td>
						<xsl:value-of select="period"/>
					</td>
					<td>
						<xsl:value-of select="income"/>
					</td>
					<td>
						<xsl:value-of select="ЗамовленняКлієнта_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Номенклатура_Назва"/>
					</td>
					<td>
						<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Склад_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Замовлено"/>
					</td>
					<td>
						<xsl:value-of select="Сума"/>
					</td>
				</tr>

			</xsl:for-each>

		</table>
		
	</xsl:template>

	<xsl:template match="РозрахункиЗКлієнтами">
		<br/>
		<h5>Розрахунки з клієнтами</h5>
		
		<table class="table table-bordered table-sm">

			<tr>
				<th>period</th>
				<th>income</th>
				<th>Контрагент</th>
				<th>Валюта</th>
				<th>Замовлено</th>
			</tr>

			<xsl:for-each select="row">

				<tr>
					<td>
						<xsl:value-of select="period"/>
					</td>
					<td>
						<xsl:value-of select="income"/>
					</td>
					<td>
						<xsl:value-of select="Контрагент_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Валюта_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Замовлено"/>
					</td>
				</tr>

			</xsl:for-each>

		</table>

	</xsl:template>

	<xsl:template match="ВільніЗалишки">
		<br/>
		<h5>Вільні залишки</h5>
		
		<table class="table table-bordered table-sm">

			<tr>
				<th>period</th>
				<th>income</th>
				<th>Номенклатура</th>
				<th>Характеристика номенклатури</th>
				<th>Склад</th>
				<th>ВНаявності</th>
				<th>ВРезервіЗіСкладу</th>
				<th>ВРезервіПідЗамовлення</th>
			</tr>

			<xsl:for-each select="row">

				<tr>
					<td>
						<xsl:value-of select="period"/>
					</td>
					<td>
						<xsl:value-of select="income"/>
					</td>
					<td>
						<xsl:value-of select="Номенклатура_Назва"/>
					</td>
					<td>
						<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Склад_Назва"/>
					</td>
					<td>
						<xsl:value-of select="ВНаявності"/>
					</td>
					<td>
						<xsl:value-of select="ВРезервіЗіСкладу"/>
					</td>
					<td>
						<xsl:value-of select="ВРезервіПідЗамовлення"/>
					</td>
				</tr>

			</xsl:for-each>

		</table>

	</xsl:template>

	<xsl:template match="ЗамовленняПостачальникам">
		<br/>
		<h5>Замовлення постачальникам</h5>

		<table class="table table-bordered table-sm">

			<tr>
				<th>period</th>
				<th>income</th>
				<th>ЗамовленняПостачальнику</th>
				<th>Номенклатура</th>
				<th>Характеристика номенклатури</th>
				<th>Склад</th>
				<th>Замовлено</th>
			</tr>

			<xsl:for-each select="row">

				<tr>
					<td>
						<xsl:value-of select="period"/>
					</td>
					<td>
						<xsl:value-of select="income"/>
					</td>
					<td>
						<xsl:value-of select="ЗамовленняПостачальнику_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Номенклатура_Назва"/>
					</td>
					<td>
						<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Склад_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Замовлено"/>
					</td>
				</tr>

			</xsl:for-each>

		</table>

	</xsl:template>

	<xsl:template match="РозрахункиЗПостачальниками">
		<br/>
		<h5>Розрахунки з постачальниками</h5>

		<table class="table table-bordered table-sm">

			<tr>
				<th>period</th>
				<th>income</th>
				<th>Контрагент</th>
				<th>Валюта</th>
				<th>Замовлено</th>
			</tr>

			<xsl:for-each select="row">

				<tr>
					<td>
						<xsl:value-of select="period"/>
					</td>
					<td>
						<xsl:value-of select="income"/>
					</td>
					<td>
						<xsl:value-of select="Контрагент_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Валюта_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Замовлено"/>
					</td>
				</tr>

			</xsl:for-each>

		</table>

	</xsl:template>

	<xsl:template match="ТовариДоПоступлення">
		<br/>
		<h5>Товари до поступлення</h5>

		<table class="table table-bordered table-sm">

			<tr>
				<th>period</th>
				<th>income</th>
				<th>Номенклатура</th>
				<th>Характеристика номенклатури</th>
				<th>Склад</th>
				<th>ДоПоступлення</th>
			</tr>

			<xsl:for-each select="row">

				<tr>
					<td>
						<xsl:value-of select="period"/>
					</td>
					<td>
						<xsl:value-of select="income"/>
					</td>
					<td>
						<xsl:value-of select="Номенклатура_Назва"/>
					</td>
					<td>
						<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Склад_Назва"/>
					</td>
					<td>
						<xsl:value-of select="ДоПоступлення"/>
					</td>
				</tr>

			</xsl:for-each>

		</table>

	</xsl:template>
	
</xsl:stylesheet>
