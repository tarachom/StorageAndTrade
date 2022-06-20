<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	
    <xsl:output method="html" indent="yes"/>

    <xsl:template match="/root">

		<xsl:apply-templates select="ТовариНаСкладах" />
		
		<xsl:apply-templates select="РухТоварів" />
		
		<xsl:apply-templates select="ЗамовленняКлієнтів" />
		
		<xsl:apply-templates select="РозрахункиЗКлієнтами" />
		
		<xsl:apply-templates select="ВільніЗалишки" />
		
		<xsl:apply-templates select="ЗамовленняПостачальникам" />
		
		<xsl:apply-templates select="РозрахункиЗПостачальниками" />
		
		<xsl:apply-templates select="ТовариДоПоступлення" />
		
    </xsl:template>
	
	<xsl:template match="ТовариНаСкладах">

		<h1>Товари на cкладах</h1>

		<table border="1" width="100%">

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

		<h1>Рух товарів</h1>

		<table border="1" width="100%">

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

		<h1>Замовлення клієнтів</h1>
		
		<table border="1" width="100%">

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

		<h1>Розрахунки з клієнтами</h1>
		
		<table border="1" width="100%">

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

		<h1>Вільні залишки</h1>
		
		<table border="1" width="100%">

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

		<h1>Замовлення постачальникам</h1>

		<table border="1" width="100%">

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

		<h1>Розрахунки з постачальниками</h1>

		<table border="1" width="100%">

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

		<h1>Товари до поступлення</h1>

		<table border="1" width="100%">

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
