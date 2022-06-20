<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>

	<xsl:template match="">
		
	</xsl:template>
	
    <xsl:template match="/root">

		<table border="1" width="100%">

			<tr>
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
	
</xsl:stylesheet>
