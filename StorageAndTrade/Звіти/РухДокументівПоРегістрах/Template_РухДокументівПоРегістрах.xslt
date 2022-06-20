<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	
    <xsl:output method="html" indent="yes"/>

    <xsl:template match="/root">

		<xsl:apply-templates select="ЗамовленняКлієнтів" />
		<xsl:apply-templates select="РозрахункиЗКлієнтами" />
		<xsl:apply-templates select="ВільніЗалишки" />
		
    </xsl:template>

	<xsl:template match="ЗамовленняКлієнтів">
		
		<table border="1" width="100%">

			<tr>
				<th>period</th>
				<th>income</th>
				<th>ЗамовленняКлієнта</th>
				<th>номенклатура</th>
				<th>характеристика номенклатури</th>
				<th>склад</th>
				<th>замовлено</th>
				<th>сума</th>
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

		<table border="1" width="100%">

			<tr>
				<th>period</th>
				<th>income</th>
				<th>Замовлення клієнта</th>
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
						<xsl:value-of select="ЗамовленняКлієнта_Назва"/>
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
	
</xsl:stylesheet>
