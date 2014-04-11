<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

  <!-- When presented with an ISO8601 date, translate it into a human readable date matching our house style-->
  <xsl:template name="ISO8601DateToHouseStyleDate">
    <xsl:param name="Date" />
    <xsl:choose>
      <xsl:when test="substring($Date,9,1) = '0'">
        <xsl:value-of select="substring($Date,10,1)" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="substring($Date,9,2)" />
      </xsl:otherwise>
    </xsl:choose>
    <xsl:text> </xsl:text>
    <xsl:choose>
      <xsl:when test="substring($Date,6,2) = '01'">January</xsl:when>
      <xsl:when test="substring($Date,6,2) = '02'">February</xsl:when>
      <xsl:when test="substring($Date,6,2) = '03'">March</xsl:when>
      <xsl:when test="substring($Date,6,2) = '04'">April</xsl:when>
      <xsl:when test="substring($Date,6,2) = '05'">May</xsl:when>
      <xsl:when test="substring($Date,6,2) = '06'">June</xsl:when>
      <xsl:when test="substring($Date,6,2) = '07'">July</xsl:when>
      <xsl:when test="substring($Date,6,2) = '08'">August</xsl:when>
      <xsl:when test="substring($Date,6,2) = '09'">September</xsl:when>
      <xsl:when test="substring($Date,6,2) = '10'">October</xsl:when>
      <xsl:when test="substring($Date,6,2) = '11'">November</xsl:when>
      <xsl:when test="substring($Date,6,2) = '12'">December</xsl:when>
    </xsl:choose>
    <xsl:text> </xsl:text>
    <xsl:value-of select="substring($Date,1,4)"/>
  </xsl:template>

<!-- When presented with an ISO8601 date, translate it into a human readable time matching our house style-->
  <xsl:template name="ISO8601DateToHouseStyleTime">
    <xsl:param name="Date" />

    <xsl:variable name="hour">
      <xsl:choose>
        <xsl:when test="substring($Date,12,2) = '00'">12</xsl:when>
        <xsl:when test="substring($Date,12,2) = '01'">1</xsl:when>
        <xsl:when test="substring($Date,12,2) = '02'">2</xsl:when>
        <xsl:when test="substring($Date,12,2) = '03'">3</xsl:when>
        <xsl:when test="substring($Date,12,2) = '04'">4</xsl:when>
        <xsl:when test="substring($Date,12,2) = '05'">5</xsl:when>
        <xsl:when test="substring($Date,12,2) = '06'">6</xsl:when>
        <xsl:when test="substring($Date,12,2) = '07'">7</xsl:when>
        <xsl:when test="substring($Date,12,2) = '08'">8</xsl:when>
        <xsl:when test="substring($Date,12,2) = '09'">9</xsl:when>
        <xsl:when test="substring($Date,12,2) = '10'">10</xsl:when>
        <xsl:when test="substring($Date,12,2) = '11'">11</xsl:when>
        <xsl:when test="substring($Date,12,2) = '12'">12</xsl:when>
        <xsl:when test="substring($Date,12,2) = '13'">1</xsl:when>
        <xsl:when test="substring($Date,12,2) = '14'">2</xsl:when>
        <xsl:when test="substring($Date,12,2) = '15'">3</xsl:when>
        <xsl:when test="substring($Date,12,2) = '16'">4</xsl:when>
        <xsl:when test="substring($Date,12,2) = '17'">5</xsl:when>
        <xsl:when test="substring($Date,12,2) = '18'">6</xsl:when>
        <xsl:when test="substring($Date,12,2) = '19'">7</xsl:when>
        <xsl:when test="substring($Date,12,2) = '20'">8</xsl:when>
        <xsl:when test="substring($Date,12,2) = '21'">9</xsl:when>
        <xsl:when test="substring($Date,12,2) = '22'">10</xsl:when>
        <xsl:when test="substring($Date,12,2) = '23'">11</xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="ampm">
      <xsl:choose>
        <xsl:when test="number(substring($Date,12,2)) > 12">pm</xsl:when>
        <xsl:otherwise>am</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="$hour"/>
    <xsl:choose>
      <xsl:when test="substring($Date,15,2) = '00'">
        <xsl:choose>
        <xsl:when test="substring($Date,12,2) = '00'">
          <xsl:text> midnight</xsl:text>
        </xsl:when>
          <xsl:when test="substring($Date,12,2) = '12'">
          <xsl:text> noon</xsl:text>
        </xsl:when>
          <xsl:otherwise><xsl:value-of select="$ampm"/></xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>.</xsl:text>
        <xsl:value-of select="substring($Date,15,2)"/>
        <xsl:value-of select="$ampm"/>
      </xsl:otherwise>
    </xsl:choose>      
  </xsl:template>

  <!-- When presented with an ISO8601 date, translate it into a human readable date and time matching our house style-->
  <xsl:template name="ISO8601DateToHouseStyleDateTime">
    <xsl:param name="Date" />
    <xsl:call-template name="ISO8601DateToHouseStyleDate">
      <xsl:with-param name="Date" select="MeetingDate" />
    </xsl:call-template>
    <xsl:text>, </xsl:text>
    <xsl:call-template name="ISO8601DateToHouseStyleTime">
      <xsl:with-param name="Date" select="MeetingDate" />
    </xsl:call-template>
  </xsl:template>

  <!-- When presented with a ISO8601 date, translate it into an RFC822 date, which is the expected format in RSS feeds -->
  <xsl:template name="ISO8601DateToRFC822Date">
    <xsl:param name="Date" />
    <xsl:choose>
      <xsl:when test="substring($Date,9,1) = '0'">
        <xsl:value-of select="substring($Date,10,1)" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="substring($Date,9,2)" />
      </xsl:otherwise>
    </xsl:choose>
    <xsl:text> </xsl:text>
    <xsl:choose>
      <xsl:when test="substring($Date,6,2) = '01'">Jan</xsl:when>
      <xsl:when test="substring($Date,6,2) = '02'">Feb</xsl:when>
      <xsl:when test="substring($Date,6,2) = '03'">Mar</xsl:when>
      <xsl:when test="substring($Date,6,2) = '04'">Apr</xsl:when>
      <xsl:when test="substring($Date,6,2) = '05'">May</xsl:when>
      <xsl:when test="substring($Date,6,2) = '06'">Jun</xsl:when>
      <xsl:when test="substring($Date,6,2) = '07'">Jul</xsl:when>
      <xsl:when test="substring($Date,6,2) = '08'">Aug</xsl:when>
      <xsl:when test="substring($Date,6,2) = '09'">Sep</xsl:when>
      <xsl:when test="substring($Date,6,2) = '10'">Oct</xsl:when>
      <xsl:when test="substring($Date,6,2) = '11'">Nov</xsl:when>
      <xsl:when test="substring($Date,6,2) = '12'">Dec</xsl:when>
    </xsl:choose>
    <xsl:text> </xsl:text>
    <xsl:value-of select="substring($Date,1,4)"/>
    <xsl:text> </xsl:text>
    <xsl:value-of select="substring($Date,12,8)" />
    <xsl:text> UT</xsl:text>
  </xsl:template>
</xsl:stylesheet>
